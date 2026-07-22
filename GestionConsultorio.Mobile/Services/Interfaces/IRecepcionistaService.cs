using GestionConsultorio.Shared.DTOs.Auth;
using GestionConsultorio.Shared.DTOs.Recepcionistas;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface IRecepcionistaService
{
    Task<ApiResponse<List<RecepcionistaDto>>> ObtenerTodosAsync();

    Task<ApiResponse<RecepcionistaDto>> ObtenerPorIdAsync(int id);

    Task<ApiResponse<RecepcionistaDto>> CrearAsync(RegistroUsuarioDto dto);

    Task<ApiResponse<RecepcionistaDto>> ActualizarAsync(int id, ActualizarRecepcionistaDto dto);

    Task<ApiResponse<bool>> EliminarAsync(int id);
}