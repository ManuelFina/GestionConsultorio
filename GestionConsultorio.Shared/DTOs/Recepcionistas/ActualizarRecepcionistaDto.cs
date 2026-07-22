namespace GestionConsultorio.Shared.DTOs.Recepcionistas;

public class ActualizarRecepcionistaDto
{
    public string NombreCompleto { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Password { get; set; }
}