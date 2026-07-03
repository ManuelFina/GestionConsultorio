using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Auth;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class SesionService : ISesionService
{
    private const string TokenKey = "auth_token";
    private const string UsuarioIdKey = "usuario_id";
    private const string NombreCompletoKey = "nombre_completo";
    private const string EmailKey = "email";
    private const string RolKey = "rol";

    public async Task GuardarSesionAsync(AuthResponseDto authResponse)
    {
        await SecureStorage.Default.SetAsync(TokenKey, authResponse.Token);

        Preferences.Default.Set(UsuarioIdKey, authResponse.UsuarioId);
        Preferences.Default.Set(NombreCompletoKey, authResponse.NombreCompleto);
        Preferences.Default.Set(EmailKey, authResponse.Email);
        Preferences.Default.Set(RolKey, authResponse.Rol);
    }

    public async Task<string?> ObtenerTokenAsync()
    {
        return await SecureStorage.Default.GetAsync(TokenKey);
    }

    public string? ObtenerRol()
    {
        var rol = Preferences.Default.Get(RolKey, string.Empty);
        return string.IsNullOrWhiteSpace(rol) ? null : rol;
    }

    public string? ObtenerNombreCompleto()
    {
        var nombreCompleto = Preferences.Default.Get(NombreCompletoKey, string.Empty);
        return string.IsNullOrWhiteSpace(nombreCompleto) ? null : nombreCompleto;
    }

    public string? ObtenerEmail()
    {
        var email = Preferences.Default.Get(EmailKey, string.Empty);
        return string.IsNullOrWhiteSpace(email) ? null : email;
    }

    public int ObtenerUsuarioId()
    {
        return Preferences.Default.Get(UsuarioIdKey, 0);
    }

    public void CerrarSesion()
    {
        SecureStorage.Default.Remove(TokenKey);

        Preferences.Default.Remove(UsuarioIdKey);
        Preferences.Default.Remove(NombreCompletoKey);
        Preferences.Default.Remove(EmailKey);
        Preferences.Default.Remove(RolKey);
    }
}