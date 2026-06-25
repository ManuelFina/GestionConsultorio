using System.Net.Http.Json;
using GestionConsultorio.Mobile.Helpers;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Auth;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class AuthService(HttpClient httpClient, ISesionService sesionService) : IAuthService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ISesionService _sesionService = sesionService;

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Auth}/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<AuthResponseDto>.Error(mensaje);
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (authResponse is null)
                return ApiResponse<AuthResponseDto>.Error("No se recibió información de inicio de sesión.");

            await _sesionService.GuardarSesionAsync(authResponse);

            return ApiResponse<AuthResponseDto>.Ok(authResponse);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<AuthResponseDto>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<AuthResponseDto>.Error("Ocurrió un error inesperado al iniciar sesión.");
        }
    }

    public async Task<ApiResponse<AuthResponseDto>> RegistrarAsync(RegistroUsuarioDto registroDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Auth}/registro", registroDto);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<AuthResponseDto>.Error(mensaje);
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (authResponse is null)
                return ApiResponse<AuthResponseDto>.Error("El usuario fue registrado, pero no se recibió la información.");

            await _sesionService.GuardarSesionAsync(authResponse);

            return ApiResponse<AuthResponseDto>.Ok(authResponse);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<AuthResponseDto>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<AuthResponseDto>.Error("Ocurrió un error inesperado al registrar el usuario.");
        }
    }
}