using GestionConsultorio.Shared.DTOs.Auth;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ResultadoOperacion<AuthResponseDto>> RegistrarAsync(RegistroUsuarioDto dto);
    Task<ResultadoOperacion<AuthResponseDto>> LoginAsync(LoginDto dto);
}