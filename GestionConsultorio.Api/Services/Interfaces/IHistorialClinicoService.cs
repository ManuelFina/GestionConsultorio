using GestionConsultorio.Shared.DTOs.HistorialesClinicos;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Interfaces;

public interface IHistorialClinicoService
{
    Task<IEnumerable<HistorialClinico>> ObtenerTodosAsync();
    Task<HistorialClinico?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<HistorialClinico>> ObtenerPorPacienteAsync(int pacienteId);
    Task<HistorialClinico?> ObtenerPorTurnoAsync(int turnoId);

    Task<ResultadoOperacion<HistorialClinico>> CrearAsync(HistorialClinico historial);
    Task<ResultadoOperacion<HistorialClinico>> ActualizarAsync(int id, HistorialClinico historial);
    Task<ResultadoOperacion<bool>> EliminarAsync(int id);
    Task<ResultadoOperacion<HistorialClinico>> AtenderTurnoAsync(AtenderTurnoDto dto);
}