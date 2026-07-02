using GestionConsultorio.Api.Data;
using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionConsultorio.Api.Repositories.Implementaciones;

public class MedicoRepository(AppDbContext context) : Repository<Medico>(context), IMedicoRepository
{
    private readonly AppDbContext _context = context;

    private IQueryable<Medico> MedicosConEspecialidad()
    {
        return _context.Medicos
            .AsNoTracking()
            .Include(m => m.Especialidad);
    }

    public async Task<IEnumerable<Medico>> ObtenerTodosConEspecialidadAsync()
    {
        return await MedicosConEspecialidad()
            .ToListAsync();
    }

    public async Task<Medico?> ObtenerPorIdConEspecialidadAsync(int id)
    {
        return await MedicosConEspecialidad()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Medico?> ObtenerPorMatriculaAsync(string matricula)
    {
        var matriculaNormalizada = matricula.Trim().ToLower();

        return await MedicosConEspecialidad()
            .FirstOrDefaultAsync(m => m.Matricula.ToLower() == matriculaNormalizada);
    }

    public async Task<bool> ExisteMatriculaAsync(string matricula)
    {
        var matriculaNormalizada = matricula.Trim().ToLower();

        return await _context.Medicos
            .AsNoTracking()
            .AnyAsync(m => m.Matricula.ToLower() == matriculaNormalizada);
    }

    public async Task<IEnumerable<Medico>> ObtenerPorEspecialidadAsync(int especialidadId)
    {
        return await MedicosConEspecialidad()
            .Where(m => m.EspecialidadId == especialidadId)
            .ToListAsync();
    }

    public async Task<Medico?> ObtenerPorEmailAsync(string email)
    {
        var emailNormalizado = email.Trim().ToLower();

        return await MedicosConEspecialidad()
            .FirstOrDefaultAsync(m => m.Email.ToLower() == emailNormalizado);
    }
}