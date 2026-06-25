using GestionConsultorio.Shared.Models;

namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);
    Task<bool> ExisteEmailAsync(string email);
    Task CrearUsuarioAsync(Usuario usuario);
}