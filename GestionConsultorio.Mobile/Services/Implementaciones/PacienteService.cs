using GestionConsultorio.Mobile.Helpers;
using GestionConsultorio.Shared.Responses;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using System.Net;
using System.Net.Http.Json;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class PacienteService(HttpClient httpClient) : IPacienteService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<Paciente>>> ObtenerTodosAsync()
    {
        try
        {
            var pacientes = await _httpClient.GetFromJsonAsync<List<Paciente>>(ApiRoutes.Pacientes);

            return ApiResponse<List<Paciente>>.Ok(pacientes ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<Paciente>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<Paciente>>.Error("Ocurrió un error inesperado al obtener los pacientes.");
        }
    }

    public async Task<ApiResponse<Paciente>> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Pacientes}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<Paciente>.Error("Paciente no encontrado.");

            if (!response.IsSuccessStatusCode)
                return ApiResponse<Paciente>.Error("No se pudo obtener el paciente.");

            var paciente = await response.Content.ReadFromJsonAsync<Paciente>();

            if (paciente is null)
                return ApiResponse<Paciente>.Error("No se recibió información del paciente.");

            return ApiResponse<Paciente>.Ok(paciente);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Paciente>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Paciente>.Error("Ocurrió un error inesperado al obtener el paciente.");
        }
    }

    public async Task<ApiResponse<Paciente>> ObtenerPorDniAsync(string dni)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Pacientes}/dni/{dni}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<Paciente>.Error("Paciente no encontrado.");

            if (!response.IsSuccessStatusCode)
                return ApiResponse<Paciente>.Error("No se pudo obtener el paciente.");

            var paciente = await response.Content.ReadFromJsonAsync<Paciente>();

            if (paciente is null)
                return ApiResponse<Paciente>.Error("No se recibió información del paciente.");

            return ApiResponse<Paciente>.Ok(paciente);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Paciente>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Paciente>.Error("Ocurrió un error inesperado al buscar el paciente.");
        }
    }

    public async Task<ApiResponse<Paciente>> CrearAsync(Paciente paciente)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Pacientes, paciente);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Paciente>.Error(mensaje);
            }

            var pacienteCreado = await response.Content.ReadFromJsonAsync<Paciente>();

            if (pacienteCreado is null)
                return ApiResponse<Paciente>.Error("El paciente fue creado, pero no se recibió la información.");

            return ApiResponse<Paciente>.Ok(pacienteCreado);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Paciente>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Paciente>.Error("Ocurrió un error inesperado al crear el paciente.");
        }
    }

    public async Task<ApiResponse<bool>> ActualizarAsync(int id, Paciente paciente)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Pacientes}/{id}", paciente);

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al actualizar el paciente.");
        }
    }

    public async Task<ApiResponse<bool>> EliminarAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Pacientes}/{id}");

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al eliminar el paciente.");
        }
    }

}