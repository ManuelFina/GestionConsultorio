using GestionConsultorio.Api.Data;
using GestionConsultorio.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class ValidacionEliminacionService(AppDbContext context) : IValidacionEliminacionService
{
    private readonly AppDbContext _context = context;

    public async Task<ResultadoValidacionEliminacion> ValidarEspecialidadAsync(int especialidadId)
    {
        var tieneMedicos = await _context.Medicos
            .AnyAsync(m => m.EspecialidadId == especialidadId);

        if (tieneMedicos)
        {
            return new ResultadoValidacionEliminacion(
                false,
                "No se puede eliminar la especialidad porque tiene médicos asociados."
            );
        }

        return new ResultadoValidacionEliminacion(true);
    }

    public async Task<ResultadoValidacionEliminacion> ValidarMedicoAsync(int medicoId)
    {
        var tieneTurnos = await _context.Turnos
            .AnyAsync(t => t.MedicoId == medicoId);

        if (tieneTurnos)
        {
            return new ResultadoValidacionEliminacion(
                false,
                "No se puede eliminar el médico porque tiene turnos asociados."
            );
        }

        return new ResultadoValidacionEliminacion(true);
    }

    public async Task<ResultadoValidacionEliminacion> ValidarPacienteAsync(int pacienteId)
    {
        var tieneTurnos = await _context.Turnos
            .AnyAsync(t => t.PacienteId == pacienteId);

        if (tieneTurnos)
        {
            return new ResultadoValidacionEliminacion(
                false,
                "No se puede eliminar el paciente porque tiene turnos asociados."
            );
        }

        var tieneHistoriales = await _context.HistorialesClinicos
            .AnyAsync(h => h.PacienteId == pacienteId);

        if (tieneHistoriales)
        {
            return new ResultadoValidacionEliminacion(
                false,
                "No se puede eliminar el paciente porque tiene historiales clínicos asociados."
            );
        }

        return new ResultadoValidacionEliminacion(true);
    }

    public async Task<ResultadoValidacionEliminacion> ValidarConsultorioAsync(int consultorioId)
    {
        var tieneTurnos = await _context.Turnos
            .AnyAsync(t => t.ConsultorioId == consultorioId);

        if (tieneTurnos)
        {
            return new ResultadoValidacionEliminacion(
                false,
                "No se puede eliminar el consultorio porque tiene turnos asociados."
            );
        }

        return new ResultadoValidacionEliminacion(true);
    }

    public async Task<ResultadoValidacionEliminacion> ValidarTurnoAsync(int turnoId)
    {
        var tieneHistorial = await _context.HistorialesClinicos
            .AnyAsync(h => h.TurnoId == turnoId);

        if (tieneHistorial)
        {
            return new ResultadoValidacionEliminacion(
                false,
                "No se puede eliminar el turno porque ya tiene un historial clínico asociado."
            );
        }

        return new ResultadoValidacionEliminacion(true);
    }
}