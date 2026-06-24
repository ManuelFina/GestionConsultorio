using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Enums;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class TurnoService(
    ITurnoRepository turnoRepository,
    IPacienteRepository pacienteRepository,
    IMedicoRepository medicoRepository,
    IRepository<Consultorio> consultorioRepository) : ITurnoService
{
    private readonly ITurnoRepository _turnoRepository = turnoRepository;
    private readonly IPacienteRepository _pacienteRepository = pacienteRepository;
    private readonly IMedicoRepository _medicoRepository = medicoRepository;
    private readonly IRepository<Consultorio> _consultorioRepository = consultorioRepository;

    public async Task<IEnumerable<Turno>> ObtenerTodosAsync()
    {
        return await _turnoRepository.ObtenerTodosAsync();
    }

    public async Task<Turno?> ObtenerPorIdAsync(int id)
    {
        return await _turnoRepository.ObtenerPorIdAsync(id);
    }

    public async Task<IEnumerable<Turno>> ObtenerPorFechaAsync(DateOnly fecha)
    {
        return await _turnoRepository.ObtenerPorFechaAsync(fecha);
    }

    public async Task<IEnumerable<Turno>> ObtenerPorMedicoAsync(int medicoId)
    {
        return await _turnoRepository.ObtenerPorMedicoAsync(medicoId);
    }

    public async Task<IEnumerable<Turno>> ObtenerPorPacienteAsync(int pacienteId)
    {
        return await _turnoRepository.ObtenerPorPacienteAsync(pacienteId);
    }

    public async Task<ResultadoOperacion<Turno>> CrearAsync(Turno turno)
    {
        var validacion = await ValidarTurnoAsync(turno);

        if (!validacion.Exitoso)
            return ResultadoOperacion<Turno>.Error(validacion.Mensaje);

        turno.Estado = EstadoTurno.Pendiente;

        await _turnoRepository.CrearAsync(turno);

        return ResultadoOperacion<Turno>.Ok(turno);
    }

    public async Task<ResultadoOperacion<Turno>> ActualizarAsync(int id, Turno turno)
    {
        if (id != turno.Id)
            return ResultadoOperacion<Turno>.Error("El ID de la ruta no coincide con el ID del turno.");

        var turnoExistente = await _turnoRepository.ObtenerPorIdAsync(id);

        if (turnoExistente is null)
            return ResultadoOperacion<Turno>.Error("Turno no encontrado.");

        var validacion = await ValidarTurnoAsync(turno);

        if (!validacion.Exitoso)
            return ResultadoOperacion<Turno>.Error(validacion.Mensaje);

        turnoExistente.PacienteId = turno.PacienteId;
        turnoExistente.MedicoId = turno.MedicoId;
        turnoExistente.ConsultorioId = turno.ConsultorioId;
        turnoExistente.Fecha = turno.Fecha;
        turnoExistente.HoraInicio = turno.HoraInicio;
        turnoExistente.HoraFin = turno.HoraFin;
        turnoExistente.MotivoConsulta = turno.MotivoConsulta;
        turnoExistente.Estado = turno.Estado;

        await _turnoRepository.ActualizarAsync(turnoExistente);

        return ResultadoOperacion<Turno>.Ok(turnoExistente);
    }

    public async Task<ResultadoOperacion<Turno>> CambiarEstadoAsync(int id, EstadoTurno nuevoEstado)
    {
        var turno = await _turnoRepository.ObtenerPorIdAsync(id);

        if (turno is null)
            return ResultadoOperacion<Turno>.Error("Turno no encontrado.");

        var validacion = ValidarCambioEstado(turno, nuevoEstado);

        if (!validacion.Exitoso)
            return ResultadoOperacion<Turno>.Error(validacion.Mensaje);

        turno.Estado = nuevoEstado;

        await _turnoRepository.ActualizarAsync(turno);

        return ResultadoOperacion<Turno>.Ok(turno);
    }

    public async Task<ResultadoOperacion<bool>> EliminarAsync(int id)
    {
        var turno = await _turnoRepository.ObtenerPorIdAsync(id);

        if (turno is null)
            return ResultadoOperacion<bool>.Error("Turno no encontrado.");

        await _turnoRepository.EliminarAsync(turno);

        return ResultadoOperacion<bool>.Ok(true);
    }

    private async Task<ResultadoOperacion<bool>> ValidarTurnoAsync(Turno turno)
    {
        if (turno.HoraInicio >= turno.HoraFin)
            return ResultadoOperacion<bool>.Error("La hora de inicio debe ser menor que la hora de fin.");

        var paciente = await _pacienteRepository.ObtenerPorIdAsync(turno.PacienteId);

        if (paciente is null)
            return ResultadoOperacion<bool>.Error("El paciente seleccionado no existe.");

        var medico = await _medicoRepository.ObtenerPorIdAsync(turno.MedicoId);

        if (medico is null)
            return ResultadoOperacion<bool>.Error("El médico seleccionado no existe.");

        var consultorio = await _consultorioRepository.ObtenerPorIdAsync(turno.ConsultorioId);

        if (consultorio is null)
            return ResultadoOperacion<bool>.Error("El consultorio seleccionado no existe.");

        var existeSuperposicion = await _turnoRepository.ExisteSuperposicionAsync(
            turno.MedicoId,
            turno.ConsultorioId,
            turno.Fecha,
            turno.HoraInicio,
            turno.HoraFin
        );

        if (existeSuperposicion)
            return ResultadoOperacion<bool>.Error("Ya existe un turno para ese médico o consultorio en el horario seleccionado.");

        return ResultadoOperacion<bool>.Ok(true);
    }

    private static ResultadoOperacion<bool> ValidarCambioEstado(Turno turno, EstadoTurno nuevoEstado)
    {
        if (turno.Estado == EstadoTurno.Cancelado && nuevoEstado == EstadoTurno.Confirmado)
            return ResultadoOperacion<bool>.Error("No se puede confirmar un turno cancelado.");

        if (turno.Estado == EstadoTurno.Cancelado && nuevoEstado == EstadoTurno.Atendido)
            return ResultadoOperacion<bool>.Error("No se puede atender un turno cancelado.");

        if (turno.Estado == EstadoTurno.Atendido && nuevoEstado == EstadoTurno.Cancelado)
            return ResultadoOperacion<bool>.Error("No se puede cancelar un turno ya atendido.");

        if (turno.Estado == EstadoTurno.Atendido && nuevoEstado == EstadoTurno.Ausente)
            return ResultadoOperacion<bool>.Error("No se puede marcar como ausente un turno ya atendido.");

        return ResultadoOperacion<bool>.Ok(true);
    }
}