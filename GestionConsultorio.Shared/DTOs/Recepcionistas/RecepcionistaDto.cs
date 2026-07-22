namespace GestionConsultorio.Shared.DTOs.Recepcionistas;

public class RecepcionistaDto
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;
}