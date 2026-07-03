using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using GestionConsultorio.Mobile.Helpers;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;
using Microsoft.AspNetCore.Components.Forms;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class ArchivoHistorialClinicoService(HttpClient httpClient) : IArchivoHistorialClinicoService
{
    private const long TamanioMaximoBytes = 10 * 1024 * 1024;

    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<ArchivoHistorialClinico>>> ObtenerPorHistorialAsync(int historialClinicoId)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                ApiRoutes.ArchivosHistorialClinico(historialClinicoId));

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<List<ArchivoHistorialClinico>>.Error("Historial clínico no encontrado.");

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<List<ArchivoHistorialClinico>>.Error(mensaje);
            }

            var archivos = await response.Content.ReadFromJsonAsync<List<ArchivoHistorialClinico>>();

            return ApiResponse<List<ArchivoHistorialClinico>>.Ok(archivos ?? []);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<List<ArchivoHistorialClinico>>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception)
        {
            return ApiResponse<List<ArchivoHistorialClinico>>.Error("Ocurrió un error inesperado al obtener los archivos.");
        }
    }

    public async Task<ApiResponse<ArchivoHistorialClinico>> SubirArchivoAsync(
        int historialClinicoId,
        IBrowserFile archivo)
    {
        try
        {
            if (archivo is null)
                return ApiResponse<ArchivoHistorialClinico>.Error("Debe seleccionar un archivo.");

            if (archivo.Size == 0)
                return ApiResponse<ArchivoHistorialClinico>.Error("El archivo está vacío.");

            if (archivo.Size > TamanioMaximoBytes)
                return ApiResponse<ArchivoHistorialClinico>.Error("El archivo no puede superar los 10 MB.");

            using var contenido = new MultipartFormDataContent();

            await using var stream = archivo.OpenReadStream(TamanioMaximoBytes);

            var archivoContent = new StreamContent(stream);
            archivoContent.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);

            contenido.Add(archivoContent, "archivo", archivo.Name);

            var response = await _httpClient.PostAsync(
                ApiRoutes.ArchivosHistorialClinico(historialClinicoId),
                contenido);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = await ApiErrorHelper.ObtenerMensajeErrorAsync(response);
                return ApiResponse<ArchivoHistorialClinico>.Error(mensaje);
            }

            var archivoSubido = await response.Content.ReadFromJsonAsync<ArchivoHistorialClinico>();

            if (archivoSubido is null)
                return ApiResponse<ArchivoHistorialClinico>.Error("El archivo fue subido, pero no se recibió la información.");

            return ApiResponse<ArchivoHistorialClinico>.Ok(archivoSubido);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<ArchivoHistorialClinico>.Error("No se pudo conectar con el servidor.");
        }
        catch (Exception ex)
        {
            return ApiResponse<ArchivoHistorialClinico>.Error($"Ocurrió un error inesperado al subir el archivo: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> EliminarAsync(int archivoId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(
                ApiRoutes.ArchivoHistorialClinico(archivoId));

            if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResponse<bool>.Error("Archivo no encontrado.");

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
            return ApiResponse<bool>.Error("Ocurrió un error inesperado al eliminar el archivo.");
        }
    }
}