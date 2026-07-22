using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface IPacienteService
{
    Task<ApiResponse<List<Paciente>>> ObtenerTodosAsync();

    Task<ApiResponse<List<Paciente>>> ObtenerInactivosAsync();

    Task<ApiResponse<Paciente>> ObtenerPorIdAsync(int id);

    Task<ApiResponse<Paciente>> CrearAsync(Paciente paciente);

    Task<ApiResponse<bool>> ActualizarAsync(int id, Paciente paciente);

    Task<ApiResponse<bool>> EliminarAsync(int id);

    Task<ApiResponse<bool>> ReactivarAsync(int id);
}