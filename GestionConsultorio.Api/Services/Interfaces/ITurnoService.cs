using GestionConsultorio.Shared.Enums;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Interfaces;

public interface ITurnoService
{
    Task<IEnumerable<Turno>> ObtenerTodosAsync();
    Task<Turno?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<Turno>> ObtenerPorFechaAsync(DateOnly fecha);
    Task<IEnumerable<Turno>> ObtenerPorMedicoAsync(int medicoId);
    Task<IEnumerable<Turno>> ObtenerPorPacienteAsync(int pacienteId);

    Task<ResultadoOperacion<Turno>> CrearAsync(Turno turno);
    Task<ResultadoOperacion<Turno>> ActualizarAsync(int id, Turno turno);
    Task<ResultadoOperacion<Turno>> CambiarEstadoAsync(int id, EstadoTurno nuevoEstado);
    Task<ResultadoOperacion<bool>> EliminarAsync(int id);

}