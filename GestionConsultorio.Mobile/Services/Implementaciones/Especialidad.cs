using System.Net;
using System.Net.Http.Json;
using GestionConsultorio.Mobile.Helpers;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class EspecialidadService(HttpClient httpClient) : IEspecialidadService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<Especialidad>>> ObtenerTodosAsync()
    {
        try
        {
            var especialidades = await _httpClient.GetFromJsonAsync<List<Especialidad>>(ApiRoutes.Especialidades);

            return ApiResponse<List<Especialidad>>.Ok(especialidades ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<Especialidad>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<Especialidad>>.Error("Ocurrió un error inesperado al obtener las especialidades.");
        }
    }

    public async Task<ApiResponse<Especialidad>> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Especialidades}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<Especialidad>.Error("Especialidad no encontrada.");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Especialidad>.Error(mensaje);
            }

            var especialidad = await response.Content.ReadFromJsonAsync<Especialidad>();

            if (especialidad is null)
                return ApiResponse<Especialidad>.Error("No se recibió información de la especialidad.");

            return ApiResponse<Especialidad>.Ok(especialidad);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Especialidad>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Especialidad>.Error("Ocurrió un error inesperado al obtener la especialidad.");
        }
    }

    public async Task<ApiResponse<Especialidad>> CrearAsync(Especialidad especialidad)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Especialidades, especialidad);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Especialidad>.Error(mensaje);
            }

            var especialidadCreada = await response.Content.ReadFromJsonAsync<Especialidad>();

            if (especialidadCreada is null)
                return ApiResponse<Especialidad>.Error("La especialidad fue creada, pero no se recibió la información.");

            return ApiResponse<Especialidad>.Ok(especialidadCreada);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Especialidad>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Especialidad>.Error("Ocurrió un error inesperado al crear la especialidad.");
        }
    }

    public async Task<ApiResponse<bool>> ActualizarAsync(int id, Especialidad especialidad)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Especialidades}/{id}", especialidad);

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al actualizar la especialidad.");
        }
    }

    public async Task<ApiResponse<bool>> EliminarAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Especialidades}/{id}");

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al eliminar la especialidad.");
        }
    }
}