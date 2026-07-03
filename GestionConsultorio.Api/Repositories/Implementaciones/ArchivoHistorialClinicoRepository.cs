using GestionConsultorio.Api.Data;
using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionConsultorio.Api.Repositories.Implementaciones;

public class ArchivoHistorialClinicoRepository(AppDbContext context)
    : Repository<ArchivoHistorialClinico>(context), IArchivoHistorialClinicoRepository
{
    private readonly AppDbContext _context = context;

    private IQueryable<ArchivoHistorialClinico> ArchivosConRelaciones()
    {
        return _context.ArchivosHistorialClinico
            .Include(a => a.HistorialClinico)
                .ThenInclude(h => h!.Paciente)
            .Include(a => a.HistorialClinico)
                .ThenInclude(h => h!.Turno)
                    .ThenInclude(t => t!.Medico);
    }

    public async Task<IEnumerable<ArchivoHistorialClinico>> ObtenerPorHistorialAsync(int historialClinicoId)
    {
        return await ArchivosConRelaciones()
            .AsNoTracking()
            .Where(a => a.HistorialClinicoId == historialClinicoId)
            .OrderByDescending(a => a.FechaCarga)
            .ToListAsync();
    }

    public async Task<ArchivoHistorialClinico?> ObtenerPorIdConHistorialAsync(int id)
    {
        return await ArchivosConRelaciones()
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}