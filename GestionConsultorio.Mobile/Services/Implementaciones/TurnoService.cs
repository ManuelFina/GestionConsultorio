using System.Net;
using System.Net.Http.Json;
using GestionConsultorio.Mobile.Helpers;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class TurnoService(HttpClient httpClient) : ITurnoService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<Turno>>> ObtenerTodosAsync()
    {
        try
        {
            var turnos = await _httpClient.GetFromJsonAsync<List<Turno>>(ApiRoutes.Turnos);

            return ApiResponse<List<Turno>>.Ok(turnos ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<Turno>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<Turno>>.Error("Ocurrió un error inesperado al obtener los turnos.");
        }
    }

    public async Task<ApiResponse<Turno>> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Turnos}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<Turno>.Error("Turno no encontrado.");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Turno>.Error(mensaje);
            }

            var turno = await response.Content.ReadFromJsonAsync<Turno>();

            if (turno is null)
                return ApiResponse<Turno>.Error("No se recibió información del turno.");

            return ApiResponse<Turno>.Ok(turno);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Turno>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Turno>.Error("Ocurrió un error inesperado al obtener el turno.");
        }
    }

    public async Task<ApiResponse<List<Turno>>> ObtenerPorFechaAsync(DateOnly fecha)
    {
        try
        {
            var fechaFormateada = fecha.ToString("yyyy-MM-dd");

            var turnos = await _httpClient.GetFromJsonAsync<List<Turno>>(
                $"{ApiRoutes.Turnos}/fecha/{fechaFormateada}"
            );

            return ApiResponse<List<Turno>>.Ok(turnos ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<Turno>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<Turno>>.Error("Ocurrió un error inesperado al obtener los turnos por fecha.");
        }
    }

    public async Task<ApiResponse<List<Turno>>> ObtenerPorMedicoAsync(int medicoId)
    {
        try
        {
            var turnos = await _httpClient.GetFromJsonAsync<List<Turno>>(
                $"{ApiRoutes.Turnos}/medico/{medicoId}"
            );

            return ApiResponse<List<Turno>>.Ok(turnos ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<Turno>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<Turno>>.Error("Ocurrió un error inesperado al obtener los turnos del médico.");
        }
    }

    public async Task<ApiResponse<List<Turno>>> ObtenerPorPacienteAsync(int pacienteId)
    {
        try
        {
            var turnos = await _httpClient.GetFromJsonAsync<List<Turno>>(
                $"{ApiRoutes.Turnos}/paciente/{pacienteId}"
            );

            return ApiResponse<List<Turno>>.Ok(turnos ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<Turno>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<Turno>>.Error("Ocurrió un error inesperado al obtener los turnos del paciente.");
        }
    }

    public async Task<ApiResponse<Turno>> CrearAsync(Turno turno)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Turnos, turno);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<Turno>.Error(mensaje);
            }

            var turnoCreado = await response.Content.ReadFromJsonAsync<Turno>();

            if (turnoCreado is null)
                return ApiResponse<Turno>.Error("El turno fue creado, pero no se recibió la información.");

            return ApiResponse<Turno>.Ok(turnoCreado);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<Turno>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<Turno>.Error("Ocurrió un error inesperado al crear el turno.");
        }
    }

    public async Task<ApiResponse<bool>> ActualizarAsync(int id, Turno turno)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Turnos}/{id}", turno);

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al actualizar el turno.");
        }
    }

    public async Task<ApiResponse<bool>> ConfirmarAsync(int id)
    {
        return await CambiarEstadoAsync($"{ApiRoutes.Turnos}/{id}/confirmar", "No se pudo confirmar el turno.");
    }

    public async Task<ApiResponse<bool>> CancelarAsync(int id)
    {
        return await CambiarEstadoAsync($"{ApiRoutes.Turnos}/{id}/cancelar", "No se pudo cancelar el turno.");
    }

    public async Task<ApiResponse<bool>> AtenderAsync(int id)
    {
        return await CambiarEstadoAsync($"{ApiRoutes.Turnos}/{id}/atender", "No se pudo marcar el turno como atendido.");
    }

    public async Task<ApiResponse<bool>> MarcarAusenteAsync(int id)
    {
        return await CambiarEstadoAsync($"{ApiRoutes.Turnos}/{id}/ausente", "No se pudo marcar el turno como ausente.");
    }

    public async Task<ApiResponse<bool>> EliminarAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Turnos}/{id}");

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al eliminar el turno.");
        }
    }

    private async Task<ApiResponse<bool>> CambiarEstadoAsync(string url, string mensajeError)
    {
        try
        {
            var response = await _httpClient.PatchAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);

                if (!string.IsNullOrWhiteSpace(mensaje))
                    return ApiResponse<bool>.Error(mensaje);

                return ApiResponse<bool>.Error(mensajeError);
            }

            return ApiResponse<bool>.Ok(true);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<bool>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<bool>.Error(mensajeError);
        }
    }
}