namespace GestionConsultorio.Shared.DTOs.Auth;

public class AuthResponseDto
{
    public int UsuarioId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}