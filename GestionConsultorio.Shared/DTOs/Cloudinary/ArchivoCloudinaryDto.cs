namespace GestionConsultorio.Api.DTOs.Cloudinary;

public class ArchivoCloudinaryDto
{
    public string NombreArchivo { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty;
    public string TipoContenido { get; set; } = string.Empty;
    public long TamanioBytes { get; set; }
}