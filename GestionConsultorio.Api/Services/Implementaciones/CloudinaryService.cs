using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GestionConsultorio.Api.DTOs.Cloudinary;
using GestionConsultorio.Api.Services.Interfaces;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class CloudinaryService(Cloudinary cloudinary) : ICloudinaryService
{
    private readonly Cloudinary _cloudinary = cloudinary;

    public async Task<ArchivoCloudinaryDto> SubirArchivoAsync(IFormFile archivo, string carpeta)
    {
        if (archivo.Length == 0)
            throw new InvalidOperationException("El archivo está vacío.");

        await using var stream = archivo.OpenReadStream();

        if (EsImagen(archivo.ContentType))
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(archivo.FileName, stream),
                Folder = carpeta,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var resultado = await _cloudinary.UploadAsync(uploadParams);

            if (resultado.Error is not null)
                throw new InvalidOperationException(resultado.Error.Message);

            return new ArchivoCloudinaryDto
            {
                NombreArchivo = archivo.FileName,
                Url = resultado.SecureUrl.ToString(),
                PublicId = resultado.PublicId,
                ResourceType = "image",
                TipoContenido = archivo.ContentType,
                TamanioBytes = archivo.Length
            };
        }

        var rawUploadParams = new RawUploadParams
        {
            File = new FileDescription(archivo.FileName, stream),
            Folder = carpeta,
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false
        };

        var rawResultado = await _cloudinary.UploadAsync(rawUploadParams);

        if (rawResultado.Error is not null)
            throw new InvalidOperationException(rawResultado.Error.Message);

        return new ArchivoCloudinaryDto
        {
            NombreArchivo = archivo.FileName,
            Url = rawResultado.SecureUrl.ToString(),
            PublicId = rawResultado.PublicId,
            ResourceType = "raw",
            TipoContenido = archivo.ContentType,
            TamanioBytes = archivo.Length
        };
    }

    public async Task<bool> EliminarArchivoAsync(string publicId, string resourceType)
    {
        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = ObtenerResourceType(resourceType)
        };

        var resultado = await _cloudinary.DestroyAsync(deletionParams);

        return resultado.Result == "ok" || resultado.Result == "not found";
    }

    private static bool EsImagen(string contentType)
    {
        return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }

    private static ResourceType ObtenerResourceType(string resourceType)
    {
        return resourceType.ToLower() switch
        {
            "image" => ResourceType.Image,
            "raw" => ResourceType.Raw,
            "video" => ResourceType.Video,
            _ => ResourceType.Raw
        };
    }
}