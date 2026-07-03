using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class ArchivoHistorialClinicoService(
    IArchivoHistorialClinicoRepository archivoRepository,
    IHistorialClinicoRepository historialRepository,
    ICloudinaryService cloudinaryService) : IArchivoHistorialClinicoService
{
    private const long TamanioMaximoBytes = 10 * 1024 * 1024;

    private readonly IArchivoHistorialClinicoRepository _archivoRepository = archivoRepository;
    private readonly IHistorialClinicoRepository _historialRepository = historialRepository;
    private readonly ICloudinaryService _cloudinaryService = cloudinaryService;

    private static readonly string[] TiposPermitidos =
    [
        "image/jpeg",
        "image/png",
        "image/webp",
        "application/pdf"
    ];

    public async Task<IEnumerable<ArchivoHistorialClinico>> ObtenerPorHistorialAsync(int historialClinicoId)
    {
        return await _archivoRepository.ObtenerPorHistorialAsync(historialClinicoId);
    }

    public async Task<ResultadoOperacion<ArchivoHistorialClinico>> SubirArchivoAsync(
        int historialClinicoId,
        IFormFile archivo)
    {
        var validacion = await ValidarArchivoAsync(historialClinicoId, archivo);

        if (!validacion.Exitoso)
            return ResultadoOperacion<ArchivoHistorialClinico>.Error(validacion.Mensaje);

        try
        {
            var carpeta = $"gestion-consultorio/historiales/{historialClinicoId}";

            var archivoSubido = await _cloudinaryService.SubirArchivoAsync(archivo, carpeta);

            var archivoHistorial = new ArchivoHistorialClinico
            {
                HistorialClinicoId = historialClinicoId,
                NombreArchivo = archivoSubido.NombreArchivo,
                Url = archivoSubido.Url,
                PublicId = archivoSubido.PublicId,
                ResourceType = archivoSubido.ResourceType,
                TipoContenido = archivoSubido.TipoContenido,
                TamanioBytes = archivoSubido.TamanioBytes,
                FechaCarga = DateTime.Now
            };

            await _archivoRepository.CrearAsync(archivoHistorial);

            return ResultadoOperacion<ArchivoHistorialClinico>.Ok(archivoHistorial);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion<ArchivoHistorialClinico>.Error(
                $"No se pudo subir el archivo: {ex.Message}");
        }
    }

    public async Task<ResultadoOperacion<bool>> EliminarAsync(int id)
    {
        var archivo = await _archivoRepository.ObtenerPorIdConHistorialAsync(id);

        if (archivo is null)
            return ResultadoOperacion<bool>.Error("Archivo no encontrado.");

        var eliminadoEnCloudinary = await _cloudinaryService.EliminarArchivoAsync(
            archivo.PublicId,
            archivo.ResourceType);

        if (!eliminadoEnCloudinary)
            return ResultadoOperacion<bool>.Error("No se pudo eliminar el archivo de Cloudinary.");

        await _archivoRepository.EliminarAsync(archivo);

        return ResultadoOperacion<bool>.Ok(true);
    }

    private async Task<ResultadoOperacion<bool>> ValidarArchivoAsync(
        int historialClinicoId,
        IFormFile archivo)
    {
        var historial = await _historialRepository.ObtenerPorIdConRelacionesAsync(historialClinicoId);

        if (historial is null)
            return ResultadoOperacion<bool>.Error("Historial clínico no encontrado.");

        if (archivo is null)
            return ResultadoOperacion<bool>.Error("Debe seleccionar un archivo.");

        if (archivo.Length == 0)
            return ResultadoOperacion<bool>.Error("El archivo está vacío.");

        if (archivo.Length > TamanioMaximoBytes)
            return ResultadoOperacion<bool>.Error("El archivo no puede superar los 10 MB.");

        if (!TiposPermitidos.Contains(archivo.ContentType))
            return ResultadoOperacion<bool>.Error("Solo se permiten archivos JPG, PNG, WEBP o PDF.");

        return ResultadoOperacion<bool>.Ok(true);
    }
}