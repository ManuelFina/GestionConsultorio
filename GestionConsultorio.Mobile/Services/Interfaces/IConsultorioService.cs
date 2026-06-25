using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface IConsultorioService
{
    Task<ApiResponse<List<Consultorio>>> ObtenerTodosAsync();
    Task<ApiResponse<Consultorio>> ObtenerPorIdAsync(int id);
    Task<ApiResponse<Consultorio>> CrearAsync(Consultorio consultorio);
    Task<ApiResponse<bool>> ActualizarAsync(int id, Consultorio consultorio);
    Task<ApiResponse<bool>> EliminarAsync(int id);
}