using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionConsultorio.Shared.Models;

public class ArchivoHistorialClinico
{
    public int Id { get; set; }

    [Required]
    public int HistorialClinicoId { get; set; }

    [JsonIgnore]
    public HistorialClinico? HistorialClinico { get; set; }

    [Required]
    public string NombreArchivo { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;

    [Required]
    public string PublicId { get; set; } = string.Empty;

    public string ResourceType { get; set; } = string.Empty;

    public string TipoContenido { get; set; } = string.Empty;

    public long TamanioBytes { get; set; }

    public DateTime FechaCarga { get; set; } = DateTime.Now;
}