using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Enums;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class HistorialClinicoService(
    IHistorialClinicoRepository historialRepository,
    IPacienteRepository pacienteRepository,
    ITurnoRepository turnoRepository) : IHistorialClinicoService
{
    private readonly IHistorialClinicoRepository _historialRepository = historialRepository;
    private readonly IPacienteRepository _pacienteRepository = pacienteRepository;
    private readonly ITurnoRepository _turnoRepository = turnoRepository;

    public async Task<IEnumerable<HistorialClinico>> ObtenerTodosAsync()
    {
        return await _historialRepository.ObtenerTodosAsync();
    }

    public async Task<HistorialClinico?> ObtenerPorIdAsync(int id)
    {
        return await _historialRepository.ObtenerPorIdAsync(id);
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

        historial.FechaRegistro = DateTime.Now;

        await _historialRepository.CrearAsync(historial);

        return ResultadoOperacion<HistorialClinico>.Ok(historial);
    }

    public async Task<ResultadoOperacion<HistorialClinico>> ActualizarAsync(int id, HistorialClinico historial)
    {
        if (id != historial.Id)
            return ResultadoOperacion<HistorialClinico>.Error("El ID de la ruta no coincide con el ID del historial clínico.");

        var historialExistente = await _historialRepository.ObtenerPorIdAsync(id);

        if (historialExistente is null)
            return ResultadoOperacion<HistorialClinico>.Error("Historial clínico no encontrado.");

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
}