using GestionConsultorio.Api.Data;
using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionConsultorio.Api.Repositories.Implementaciones;

public class HistorialClinicoRepository(AppDbContext context)
    : Repository<HistorialClinico>(context), IHistorialClinicoRepository
{
    private readonly AppDbContext _context = context;

    private IQueryable<HistorialClinico> HistorialesConRelaciones()
    {
        return _context.HistorialesClinicos
            .AsNoTracking()
            .Include(h => h.Paciente)
            .Include(h => h.Turno!)
                .ThenInclude(t => t.Medico)
            .Include(h => h.Turno!)
                .ThenInclude(t => t.Consultorio)
            .Include(h => h.Archivos);
    }

    public async Task<IEnumerable<HistorialClinico>> ObtenerTodosConRelacionesAsync()
    {
        return await HistorialesConRelaciones()
            .OrderByDescending(h => h.FechaRegistro)
            .ToListAsync();
    }

    public async Task<HistorialClinico?> ObtenerPorIdConRelacionesAsync(int id)
    {
        return await HistorialesConRelaciones()
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<IEnumerable<HistorialClinico>> ObtenerPorPacienteAsync(int pacienteId)
    {
        return await HistorialesConRelaciones()
            .Where(h => h.PacienteId == pacienteId)
            .OrderByDescending(h => h.FechaRegistro)
            .ToListAsync();
    }

    public async Task<HistorialClinico?> ObtenerPorTurnoAsync(int turnoId)
    {
        return await HistorialesConRelaciones()
            .FirstOrDefaultAsync(h => h.TurnoId == turnoId);
    }

    public async Task<bool> ExisteHistorialParaTurnoAsync(int turnoId)
    {
        return await _context.HistorialesClinicos
            .AsNoTracking()
            .AnyAsync(h => h.TurnoId == turnoId);
    }
}