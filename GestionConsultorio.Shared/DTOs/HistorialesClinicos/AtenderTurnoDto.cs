using System.ComponentModel.DataAnnotations;

namespace GestionConsultorio.Shared.DTOs.HistorialesClinicos;

public class AtenderTurnoDto
{
    [Required(ErrorMessage = "El turno es obligatorio.")]
    public int TurnoId { get; set; }

    [Required(ErrorMessage = "El diagnóstico es obligatorio.")]
    public string Diagnostico { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tratamiento es obligatorio.")]
    public string Tratamiento { get; set; } = string.Empty;

    public string Observaciones { get; set; } = string.Empty;
}