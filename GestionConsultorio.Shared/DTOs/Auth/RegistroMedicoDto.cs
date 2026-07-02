using System.ComponentModel.DataAnnotations;

namespace GestionConsultorio.Shared.DTOs.Medicos;

public class RegistroMedicoDto
{
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "La matrícula es obligatoria.")]
    public string Matricula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar una especialidad.")]
    public int EspecialidadId { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;
}