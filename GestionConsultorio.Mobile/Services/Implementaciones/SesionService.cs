using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Auth;
using Microsoft.Maui.Storage;

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
        await SecureStorage.SetAsync(TokenKey, authResponse.Token);

        Preferences.Set(UsuarioIdKey, authResponse.UsuarioId);
        Preferences.Set(NombreCompletoKey, authResponse.NombreCompleto);
        Preferences.Set(EmailKey, authResponse.Email);
        Preferences.Set(RolKey, authResponse.Rol);
    }

    public async Task<string?> ObtenerTokenAsync()
    {
        return await SecureStorage.GetAsync(TokenKey);
    }

    public string? ObtenerRol()
    {
        return Preferences.Get(RolKey, null);
    }

    public string? ObtenerNombreCompleto()
    {
        return Preferences.Get(NombreCompletoKey, null);
    }

    public string? ObtenerEmail()
    {
        return Preferences.Get(EmailKey, null);
    }

    public int ObtenerUsuarioId()
    {
        return Preferences.Get(UsuarioIdKey, 0);
    }

    public void CerrarSesion()
    {
        SecureStorage.Remove(TokenKey);

        Preferences.Remove(UsuarioIdKey);
        Preferences.Remove(NombreCompletoKey);
        Preferences.Remove(EmailKey);
        Preferences.Remove(RolKey);
    }
}