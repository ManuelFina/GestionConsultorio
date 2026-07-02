using System.Security.Claims;
using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.HistorialesClinicos;
using GestionConsultorio.Shared.Enums;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class HistorialClinicoService(
    IHistorialClinicoRepository historialRepository,
    IPacienteRepository pacienteRepository,
    ITurnoRepository turnoRepository,
    IMedicoRepository medicoRepository,
    IHttpContextAccessor httpContextAccessor) : IHistorialClinicoService
{
    private readonly IHistorialClinicoRepository _historialRepository = historialRepository;
    private readonly IPacienteRepository _pacienteRepository = pacienteRepository;
    private readonly ITurnoRepository _turnoRepository = turnoRepository;
    private readonly IMedicoRepository _medicoRepository = medicoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<IEnumerable<HistorialClinico>> ObtenerTodosAsync()
    {
        return await _historialRepository.ObtenerTodosConRelacionesAsync();
    }

    public async Task<HistorialClinico?> ObtenerPorIdAsync(int id)
    {
        return await _historialRepository.ObtenerPorIdConRelacionesAsync(id);
    }

    public async Task<IEnumerable<HistorialClinico>> ObtenerPorPacienteAsync(int pacienteId)
    {
        return await _historialRepository.ObtenerPorPacienteAsync(pacienteId);
    }

    public async Task<HistorialClinico?> ObtenerPorTurnoAsync(int turnoId)
    {
        return await _historialRepository.ObtenerPorTurnoAsync(turnoId);
    }

    public async Task<ResultadoOperacion<HistorialClinico>> CrearAsync(HistorialClinico historial)
    {
        var validacion = await ValidarHistorialParaCrearAsync(historial);

        if (!validacion.Exitoso)
            return ResultadoOperacion<HistorialClinico>.Error(validacion.Mensaje);

        NormalizarHistorial(historial);

        historial.FechaRegistro = DateTime.Now;

        await _historialRepository.CrearAsync(historial);

        return ResultadoOperacion<HistorialClinico>.Ok(historial);
    }

    public async Task<ResultadoOperacion<HistorialClinico>> AtenderTurnoAsync(AtenderTurnoDto dto)
    {
        NormalizarAtencion(dto);

        if (dto.TurnoId <= 0)
            return ResultadoOperacion<HistorialClinico>.Error("El turno es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Diagnostico))
            return ResultadoOperacion<HistorialClinico>.Error("El diagnóstico es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Tratamiento))
            return ResultadoOperacion<HistorialClinico>.Error("El tratamiento es obligatorio.");

        var turno = await _turnoRepository.ObtenerPorIdAsync(dto.TurnoId);

        if (turno is null)
            return ResultadoOperacion<HistorialClinico>.Error("Turno no encontrado.");

        var paciente = await _pacienteRepository.ObtenerPorIdAsync(turno.PacienteId);

        if (paciente is null)
            return ResultadoOperacion<HistorialClinico>.Error("El paciente del turno no existe.");

        if (turno.Estado == EstadoTurno.Cancelado)
            return ResultadoOperacion<HistorialClinico>.Error("No se puede atender un turno cancelado.");

        if (turno.Estado == EstadoTurno.Atendido)
            return ResultadoOperacion<HistorialClinico>.Error("El turno ya fue atendido.");

        var existeHistorial = await _historialRepository.ExisteHistorialParaTurnoAsync(turno.Id);

        if (existeHistorial)
            return ResultadoOperacion<HistorialClinico>.Error("Ya existe un historial clínico registrado para este turno.");

        if (UsuarioEsMedico())
        {
            var medico = await ObtenerMedicoLogueadoAsync();

            if (medico is null)
                return ResultadoOperacion<HistorialClinico>.Error("No se encontró el médico asociado al usuario logueado.");

            if (turno.MedicoId != medico.Id)
                return ResultadoOperacion<HistorialClinico>.Error("No tenés permisos para atender este turno.");
        }

        var historial = new HistorialClinico
        {
            PacienteId = turno.PacienteId,
            TurnoId = turno.Id,
            FechaRegistro = DateTime.Now,
            Diagnostico = dto.Diagnostico,
            Tratamiento = dto.Tratamiento,
            Observaciones = dto.Observaciones
        };

        turno.Estado = EstadoTurno.Atendido;

        await _historialRepository.CrearAsync(historial);
        await _turnoRepository.ActualizarAsync(turno);

        var historialCreado = await _historialRepository.ObtenerPorIdConRelacionesAsync(historial.Id);

        return ResultadoOperacion<HistorialClinico>.Ok(historialCreado ?? historial);
    }

    public async Task<ResultadoOperacion<HistorialClinico>> ActualizarAsync(int id, HistorialClinico historial)
    {
        if (id != historial.Id)
            return ResultadoOperacion<HistorialClinico>.Error("El ID de la ruta no coincide con el ID del historial clínico.");

        var historialExistente = await _historialRepository.ObtenerPorIdAsync(id);

        if (historialExistente is null)
            return ResultadoOperacion<HistorialClinico>.Error("Historial clínico no encontrado.");

        NormalizarHistorial(historial);

        if (string.IsNullOrWhiteSpace(historial.Diagnostico))
            return ResultadoOperacion<HistorialClinico>.Error("El diagnóstico es obligatorio.");

        if (string.IsNullOrWhiteSpace(historial.Tratamiento))
            return ResultadoOperacion<HistorialClinico>.Error("El tratamiento es obligatorio.");

        historialExistente.Diagnostico = historial.Diagnostico;
        historialExistente.Tratamiento = historial.Tratamiento;
        historialExistente.Observaciones = historial.Observaciones;

        await _historialRepository.ActualizarAsync(historialExistente);

        return ResultadoOperacion<HistorialClinico>.Ok(historialExistente);
    }

    public async Task<ResultadoOperacion<bool>> EliminarAsync(int id)
    {
        var historial = await _historialRepository.ObtenerPorIdAsync(id);

        if (historial is null)
            return ResultadoOperacion<bool>.Error("Historial clínico no encontrado.");

        await _historialRepository.EliminarAsync(historial);

        return ResultadoOperacion<bool>.Ok(true);
    }

    private async Task<ResultadoOperacion<bool>> ValidarHistorialParaCrearAsync(HistorialClinico historial)
    {
        var paciente = await _pacienteRepository.ObtenerPorIdAsync(historial.PacienteId);

        if (paciente is null)
            return ResultadoOperacion<bool>.Error("El paciente seleccionado no existe.");

        var turno = await _turnoRepository.ObtenerPorIdAsync(historial.TurnoId);

        if (turno is null)
            return ResultadoOperacion<bool>.Error("El turno seleccionado no existe.");

        if (turno.PacienteId != historial.PacienteId)
            return ResultadoOperacion<bool>.Error("El turno no pertenece al paciente seleccionado.");

        if (turno.Estado != EstadoTurno.Atendido)
            return ResultadoOperacion<bool>.Error("Solo se puede registrar historial clínico para turnos atendidos.");

        var existeHistorial = await _historialRepository.ExisteHistorialParaTurnoAsync(historial.TurnoId);

        if (existeHistorial)
            return ResultadoOperacion<bool>.Error("Ya existe un historial clínico registrado para este turno.");

        return ResultadoOperacion<bool>.Ok(true);
    }

    private ClaimsPrincipal UsuarioActual =>
        _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

    private bool UsuarioEsMedico()
    {
        return UsuarioActual.IsInRole("Medico");
    }

    private string? ObtenerEmailUsuario()
    {
        return UsuarioActual.FindFirstValue(ClaimTypes.Email)
            ?? UsuarioActual.FindFirstValue("email");
    }

    private async Task<Medico?> ObtenerMedicoLogueadoAsync()
    {
        var email = ObtenerEmailUsuario();

        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _medicoRepository.ObtenerPorEmailAsync(email);
    }

    private static void NormalizarAtencion(AtenderTurnoDto dto)
    {
        dto.Diagnostico = dto.Diagnostico?.Trim() ?? string.Empty;
        dto.Tratamiento = dto.Tratamiento?.Trim() ?? string.Empty;
        dto.Observaciones = dto.Observaciones?.Trim() ?? string.Empty;
    }

    private static void NormalizarHistorial(HistorialClinico historial)
    {
        historial.Diagnostico = historial.Diagnostico?.Trim() ?? string.Empty;
        historial.Tratamiento = historial.Tratamiento?.Trim() ?? string.Empty;
        historial.Observaciones = historial.Observaciones?.Trim() ?? string.Empty;
    }
}