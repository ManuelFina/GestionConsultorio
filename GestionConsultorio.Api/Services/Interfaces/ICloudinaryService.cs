using GestionConsultorio.Api.DTOs.Cloudinary;

namespace GestionConsultorio.Api.Services.Interfaces;

public interface ICloudinaryService
{
    Task<ArchivoCloudinaryDto> SubirArchivoAsync(IFormFile archivo, string carpeta);
    Task<bool> EliminarArchivoAsync(string publicId, string resourceType);
}