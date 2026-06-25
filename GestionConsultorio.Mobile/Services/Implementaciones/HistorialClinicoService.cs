using System.Net;
using System.Net.Http.Json;
using GestionConsultorio.Mobile.Helpers;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class HistorialClinicoService(HttpClient httpClient) : IHistorialClinicoService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<HistorialClinico>>> ObtenerTodosAsync()
    {
        try
        {
            var historiales = await _httpClient.GetFromJsonAsync<List<HistorialClinico>>(
                ApiRoutes.HistorialesClinicos
            );

            return ApiResponse<List<HistorialClinico>>.Ok(historiales ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<HistorialClinico>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<HistorialClinico>>.Error("Ocurrió un error inesperado al obtener los historiales clínicos.");
        }
    }

    public async Task<ApiResponse<HistorialClinico>> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.HistorialesClinicos}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<HistorialClinico>.Error("Historial clínico no encontrado.");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<HistorialClinico>.Error(mensaje);
            }

            var historial = await response.Content.ReadFromJsonAsync<HistorialClinico>();

            if (historial is null)
                return ApiResponse<HistorialClinico>.Error("No se recibió información del historial clínico.");

            return ApiResponse<HistorialClinico>.Ok(historial);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<HistorialClinico>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<HistorialClinico>.Error("Ocurrió un error inesperado al obtener el historial clínico.");
        }
    }

    public async Task<ApiResponse<List<HistorialClinico>>> ObtenerPorPacienteAsync(int pacienteId)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"{ApiRoutes.HistorialesClinicos}/paciente/{pacienteId}"
            );

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<List<HistorialClinico>>.Error("Paciente no encontrado.");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<List<HistorialClinico>>.Error(mensaje);
            }

            var historiales = await response.Content.ReadFromJsonAsync<List<HistorialClinico>>();

            return ApiResponse<List<HistorialClinico>>.Ok(historiales ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<HistorialClinico>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<HistorialClinico>>.Error("Ocurrió un error inesperado al obtener los historiales del paciente.");
        }
    }

    public async Task<ApiResponse<HistorialClinico>> ObtenerPorTurnoAsync(int turnoId)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"{ApiRoutes.HistorialesClinicos}/turno/{turnoId}"
            );

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<HistorialClinico>.Error("Historial clínico no encontrado para ese turno.");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<HistorialClinico>.Error(mensaje);
            }

            var historial = await response.Content.ReadFromJsonAsync<HistorialClinico>();

            if (historial is null)
                return ApiResponse<HistorialClinico>.Error("No se recibió información del historial clínico.");

            return ApiResponse<HistorialClinico>.Ok(historial);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<HistorialClinico>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<HistorialClinico>.Error("Ocurrió un error inesperado al obtener el historial del turno.");
        }
    }

    public async Task<ApiResponse<HistorialClinico>> CrearAsync(HistorialClinico historial)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.HistorialesClinicos, historial);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<HistorialClinico>.Error(mensaje);
            }

            var historialCreado = await response.Content.ReadFromJsonAsync<HistorialClinico>();

            if (historialCreado is null)
                return ApiResponse<HistorialClinico>.Error("El historial clínico fue creado, pero no se recibió la información.");

            return ApiResponse<HistorialClinico>.Ok(historialCreado);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<HistorialClinico>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<HistorialClinico>.Error("Ocurrió un error inesperado al crear el historial clínico.");
        }
    }

    public async Task<ApiResponse<bool>> ActualizarAsync(int id, HistorialClinico historial)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"{ApiRoutes.HistorialesClinicos}/{id}",
                historial
            );

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al actualizar el historial clínico.");
        }
    }

    public async Task<ApiResponse<bool>> EliminarAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.HistorialesClinicos}/{id}");

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al eliminar el historial clínico.");
        }
    }
}