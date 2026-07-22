using GestionConsultorio.Shared.DTOs.Auth;
using GestionConsultorio.Shared.DTOs.Recepcionistas;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Interfaces;

public interface IRecepcionistaService
{
    Task<ResultadoOperacion<List<RecepcionistaDto>>> ObtenerTodosAsync();

    Task<ResultadoOperacion<RecepcionistaDto>> ObtenerPorIdAsync(int id);

    Task<ResultadoOperacion<RecepcionistaDto>> CrearAsync(RegistroUsuarioDto dto);

    Task<ResultadoOperacion<RecepcionistaDto>> ActualizarAsync(int id, ActualizarRecepcionistaDto dto);

    Task<ResultadoOperacion<bool>> EliminarAsync(int id);
}