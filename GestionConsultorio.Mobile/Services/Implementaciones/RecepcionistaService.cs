using System.Net.Http.Json;
using GestionConsultorio.Mobile.Helpers;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Auth;
using GestionConsultorio.Shared.DTOs.Recepcionistas;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class RecepcionistaService(HttpClient httpClient) : IRecepcionistaService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<RecepcionistaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Recepcionistas);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<List<RecepcionistaDto>>.Error(mensaje);
            }

            var recepcionistas = await response.Content.ReadFromJsonAsync<List<RecepcionistaDto>>();

            return ApiResponse<List<RecepcionistaDto>>.Ok(
                recepcionistas ?? new List<RecepcionistaDto>());
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<RecepcionistaDto>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<RecepcionistaDto>>.Error("Ocurrió un error inesperado al obtener las recepcionistas.");
        }
    }

    public async Task<ApiResponse<RecepcionistaDto>> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Recepcionistas}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<RecepcionistaDto>.Error(mensaje);
            }

            var recepcionista = await response.Content.ReadFromJsonAsync<RecepcionistaDto>();

            if (recepcionista is null)
                return ApiResponse<RecepcionistaDto>.Error("No se recibió información de la recepcionista.");

            return ApiResponse<RecepcionistaDto>.Ok(recepcionista);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<RecepcionistaDto>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<RecepcionistaDto>.Error("Ocurrió un error inesperado al obtener la recepcionista.");
        }
    }

    public async Task<ApiResponse<RecepcionistaDto>> CrearAsync(RegistroUsuarioDto dto)
    {
        try
        {
            dto.Rol = "Recepcionista";

            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Recepcionistas, dto);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<RecepcionistaDto>.Error(mensaje);
            }

            var recepcionista = await response.Content.ReadFromJsonAsync<RecepcionistaDto>();

            if (recepcionista is null)
                return ApiResponse<RecepcionistaDto>.Error("La recepcionista fue registrada, pero no se recibió la información.");

            return ApiResponse<RecepcionistaDto>.Ok(recepcionista);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<RecepcionistaDto>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<RecepcionistaDto>.Error("Ocurrió un error inesperado al registrar la recepcionista.");
        }
    }

    public async Task<ApiResponse<RecepcionistaDto>> ActualizarAsync(int id, ActualizarRecepcionistaDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Recepcionistas}/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<RecepcionistaDto>.Error(mensaje);
            }

            var recepcionista = await response.Content.ReadFromJsonAsync<RecepcionistaDto>();

            if (recepcionista is null)
                return ApiResponse<RecepcionistaDto>.Error("La recepcionista fue actualizada, pero no se recibió la información.");

            return ApiResponse<RecepcionistaDto>.Ok(recepcionista);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<RecepcionistaDto>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<RecepcionistaDto>.Error("Ocurrió un error inesperado al actualizar la recepcionista.");
        }
    }

    public async Task<ApiResponse<bool>> EliminarAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Recepcionistas}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<bool>.Error(mensaje);
            }

            return ApiResponse<bool>.Ok(true);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<bool>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al eliminar la recepcionista.");
        }
    }
}