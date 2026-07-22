using GestionConsultorio.Shared.Models;

namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface ITurnoRepository : IRepository<Turno>
{
    Task<Turno?> ObtenerPorIdConRelacionesAsync(int id);
    Task<IEnumerable<Turno>> ObtenerPorFechaAsync(DateOnly fecha);
    Task<IEnumerable<Turno>> ObtenerPorMedicoAsync(int medicoId);
    Task<IEnumerable<Turno>> ObtenerPorPacienteAsync(int pacienteId);

    Task<bool> ExisteSuperposicionAsync(
        int medicoId,
        int consultorioId,
        DateOnly fecha,
        TimeOnly horaInicio,
        TimeOnly horaFin,
        int? turnoIdExcluir = null);
    Task MarcarTurnosVencidosComoAusentesAsync();
}