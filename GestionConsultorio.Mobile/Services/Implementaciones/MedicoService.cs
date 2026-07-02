using GestionConsultorio.Mobile.Helpers;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Medicos;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;
using System.Net;
using System.Net.Http.Json;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class MedicoService(HttpClient httpClient) : IMedicoService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<Medico>>> ObtenerTodosAsync()
    {
        try
        {
            var medicos = await _httpClient.GetFromJsonAsync<List<Medico>>(ApiRoutes.Medicos);

            return ApiResponse<List<Medico>>.Ok(medicos ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<Medico>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<Medico>>.Error("Ocurrió un error inesperado al obtener los médicos.");
        }
    }

    public async Task<ApiResponse<Medico>> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Medicos}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<Medico>.Error("Médico no encontrado.");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Medico>.Error(mensaje);
            }

            var medico = await response.Content.ReadFromJsonAsync<Medico>();

            if (medico is null)
                return ApiResponse<Medico>.Error("No se recibió información del médico.");

            return ApiResponse<Medico>.Ok(medico);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Medico>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Medico>.Error("Ocurrió un error inesperado al obtener el médico.");
        }
    }

    public async Task<ApiResponse<Medico>> ObtenerPorMatriculaAsync(string matricula)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Medicos}/matricula/{matricula}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<Medico>.Error("Médico no encontrado.");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Medico>.Error(mensaje);
            }

            var medico = await response.Content.ReadFromJsonAsync<Medico>();

            if (medico is null)
                return ApiResponse<Medico>.Error("No se recibió información del médico.");

            return ApiResponse<Medico>.Ok(medico);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Medico>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Medico>.Error("Ocurrió un error inesperado al buscar el médico.");
        }
    }

    public async Task<ApiResponse<List<Medico>>> ObtenerPorEspecialidadAsync(int especialidadId)
    {
        try
        {
            var medicos = await _httpClient.GetFromJsonAsync<List<Medico>>(
                $"{ApiRoutes.Medicos}/especialidad/{especialidadId}"
            );

            return ApiResponse<List<Medico>>.Ok(medicos ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<Medico>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<Medico>>.Error("Ocurrió un error inesperado al obtener los médicos por especialidad.");
        }
    }

    public async Task<ApiResponse<Medico>> RegistrarAsync(RegistroMedicoDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Medicos, dto);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Medico>.Error(mensaje);
            }

            var medicoCreado = await response.Content.ReadFromJsonAsync<Medico>();

            if (medicoCreado is null)
                return ApiResponse<Medico>.Error("El médico fue registrado, pero no se recibió la información.");

            return ApiResponse<Medico>.Ok(medicoCreado);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Medico>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Medico>.Error("Ocurrió un error inesperado al registrar el médico.");
        }
    }

    public async Task<ApiResponse<bool>> ActualizarAsync(int id, Medico medico)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Medicos}/{id}", medico);

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al actualizar el médico.");
        }
    }

    public async Task<ApiResponse<bool>> EliminarAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Medicos}/{id}");

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al eliminar el médico.");
        }
    }
}