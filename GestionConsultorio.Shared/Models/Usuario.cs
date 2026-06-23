namespace GestionConsultorio.Shared.Models;

public class Usuario
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;
}