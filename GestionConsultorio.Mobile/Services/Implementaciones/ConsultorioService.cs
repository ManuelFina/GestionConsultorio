using System.Net;
using System.Net.Http.Json;
using GestionConsultorio.Mobile.Helpers;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class ConsultorioService(HttpClient httpClient) : IConsultorioService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<Consultorio>>> ObtenerTodosAsync()
    {
        try
        {
            var consultorios = await _httpClient.GetFromJsonAsync<List<Consultorio>>(ApiRoutes.Consultorios);

            return ApiResponse<List<Consultorio>>.Ok(consultorios ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<Consultorio>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<Consultorio>>.Error("Ocurrió un error inesperado al obtener los consultorios.");
        }
    }

    public async Task<ApiResponse<Consultorio>> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Consultorios}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<Consultorio>.Error("Consultorio no encontrado.");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Consultorio>.Error(mensaje);
            }

            var consultorio = await response.Content.ReadFromJsonAsync<Consultorio>();

            if (consultorio is null)
                return ApiResponse<Consultorio>.Error("No se recibió información del consultorio.");

            return ApiResponse<Consultorio>.Ok(consultorio);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Consultorio>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Consultorio>.Error("Ocurrió un error inesperado al obtener el consultorio.");
        }
    }

    public async Task<ApiResponse<Consultorio>> CrearAsync(Consultorio consultorio)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Consultorios, consultorio);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Consultorio>.Error(mensaje);
            }

            var consultorioCreado = await response.Content.ReadFromJsonAsync<Consultorio>();

            if (consultorioCreado is null)
                return ApiResponse<Consultorio>.Error("El consultorio fue creado, pero no se recibió la información.");

            return ApiResponse<Consultorio>.Ok(consultorioCreado);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Consultorio>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Consultorio>.Error("Ocurrió un error inesperado al crear el consultorio.");
        }
    }

    public async Task<ApiResponse<bool>> ActualizarAsync(int id, Consultorio consultorio)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Consultorios}/{id}", consultorio);

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al actualizar el consultorio.");
        }
    }

    public async Task<ApiResponse<bool>> EliminarAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Consultorios}/{id}");

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al eliminar el consultorio.");
        }
    }
}