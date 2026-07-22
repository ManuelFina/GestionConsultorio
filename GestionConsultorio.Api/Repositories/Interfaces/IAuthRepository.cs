using GestionConsultorio.Shared.Models;

namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);

    Task<bool> ExisteEmailAsync(string email);

    Task CrearUsuarioAsync(Usuario usuario);

    Task<List<Usuario>> ObtenerRecepcionistasAsync();

    Task<Usuario?> ObtenerRecepcionistaPorIdAsync(int id);

    Task ActualizarUsuarioAsync(Usuario usuario);

    Task EliminarUsuarioAsync(Usuario usuario);
}