using GestionConsultorio.Shared.DTOs.Auth;

namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface ISesionService
{
    Task GuardarSesionAsync(AuthResponseDto authResponse);
    Task<string?> ObtenerTokenAsync();
    string? ObtenerRol();
    string? ObtenerNombreCompleto();
    string? ObtenerEmail();
    int ObtenerUsuarioId();
    void CerrarSesion();
}