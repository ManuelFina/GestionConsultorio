using GestionConsultorio.Api.Data;
using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Shared.Enums;
using GestionConsultorio.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionConsultorio.Api.Repositories.Implementaciones;

public class TurnoRepository(AppDbContext context) : Repository<Turno>(context), ITurnoRepository
{
    private readonly AppDbContext _context = context;

    private IQueryable<Turno> TurnosConRelaciones()
    {
        return _context.Turnos
            .Include(t => t.Paciente)
            .Include(t => t.Medico!)
                .ThenInclude(m => m.Especialidad)
            .Include(t => t.Consultorio);
    }

    public async Task<Turno?> ObtenerPorIdConRelacionesAsync(int id)
    {
        return await TurnosConRelaciones()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Turno>> ObtenerPorFechaAsync(DateOnly fecha)
    {
        return await TurnosConRelaciones()
            .Where(t => t.Fecha == fecha)
            .OrderBy(t => t.HoraInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<Turno>> ObtenerPorMedicoAsync(int medicoId)
    {
        return await TurnosConRelaciones()
            .Where(t => t.MedicoId == medicoId)
            .OrderBy(t => t.Fecha)
            .ThenBy(t => t.HoraInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<Turno>> ObtenerPorPacienteAsync(int pacienteId)
    {
        return await TurnosConRelaciones()
            .Where(t => t.PacienteId == pacienteId)
            .OrderByDescending(t => t.Fecha)
            .ThenByDescending(t => t.HoraInicio)
            .ToListAsync();
    }

    public async Task<bool> ExisteSuperposicionAsync(
        int medicoId,
        int consultorioId,
        DateOnly fecha,
        TimeOnly horaInicio,
        TimeOnly horaFin,
        int? turnoIdExcluir = null)
    {
        return await _context.Turnos.AnyAsync(t =>
            (!turnoIdExcluir.HasValue || t.Id != turnoIdExcluir.Value) &&
            t.Fecha == fecha &&
            t.Estado != EstadoTurno.Cancelado &&
            t.Estado != EstadoTurno.Ausente &&
            (t.MedicoId == medicoId || t.ConsultorioId == consultorioId) &&
            t.HoraInicio < horaFin &&
            horaInicio < t.HoraFin);
    }

    public async Task MarcarTurnosVencidosComoAusentesAsync()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Now);
        var horaActual = TimeOnly.FromDateTime(DateTime.Now);

        var turnosVencidos = await _context.Turnos
            .Where(t =>
                (t.Estado == EstadoTurno.Pendiente || t.Estado == EstadoTurno.Confirmado) &&
                (
                    t.Fecha < hoy ||
                    (t.Fecha == hoy && t.HoraFin <= horaActual)
                ))
            .ToListAsync();

        if (!turnosVencidos.Any())
            return;

        foreach (var turno in turnosVencidos)
        {
            turno.Estado = EstadoTurno.Ausente;
        }

        await _context.SaveChangesAsync();
    }
}