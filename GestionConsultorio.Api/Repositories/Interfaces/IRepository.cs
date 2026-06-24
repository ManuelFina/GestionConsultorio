namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> ObtenerTodosAsync();
    Task<T?> ObtenerPorIdAsync(int id);
    Task CrearAsync(T entidad);
    Task ActualizarAsync(T entidad);
    Task EliminarAsync(T entidad);
}