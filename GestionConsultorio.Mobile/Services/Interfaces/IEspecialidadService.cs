using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface IEspecialidadService
{
    Task<ApiResponse<List<Especialidad>>> ObtenerTodosAsync();
    Task<ApiResponse<Especialidad>> ObtenerPorIdAsync(int id);
    Task<ApiResponse<Especialidad>> CrearAsync(Especialidad especialidad);
    Task<ApiResponse<bool>> ActualizarAsync(int id, Especialidad especialidad);
    Task<ApiResponse<bool>> EliminarAsync(int id);
}