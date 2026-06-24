using GestionConsultorio.Api.Data;
using Microsoft.EntityFrameworkCore;
using GestionConsultorio.Api.Repositories.Interfaces;

namespace GestionConsultorio.Api.Repositories.Implementaciones;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> ObtenerTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> ObtenerPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task CrearAsync(T entidad)
    {
        await _dbSet.AddAsync(entidad);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(T entidad)
    {
        _dbSet.Update(entidad);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(T entidad)
    {
        _dbSet.Remove(entidad);
        await _context.SaveChangesAsync();
    }
}