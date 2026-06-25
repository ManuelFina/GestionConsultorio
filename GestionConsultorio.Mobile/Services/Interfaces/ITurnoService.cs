using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface ITurnoService
{
    Task<ApiResponse<List<Turno>>> ObtenerTodosAsync();
    Task<ApiResponse<Turno>> ObtenerPorIdAsync(int id);
    Task<ApiResponse<List<Turno>>> ObtenerPorFechaAsync(DateOnly fecha);
    Task<ApiResponse<List<Turno>>> ObtenerPorMedicoAsync(int medicoId);
    Task<ApiResponse<List<Turno>>> ObtenerPorPacienteAsync(int pacienteId);

    Task<ApiResponse<Turno>> CrearAsync(Turno turno);
    Task<ApiResponse<bool>> ActualizarAsync(int id, Turno turno);
    Task<ApiResponse<bool>> ConfirmarAsync(int id);
    Task<ApiResponse<bool>> CancelarAsync(int id);
    Task<ApiResponse<bool>> AtenderAsync(int id);
    Task<ApiResponse<bool>> MarcarAusenteAsync(int id);
    Task<ApiResponse<bool>> EliminarAsync(int id);
}