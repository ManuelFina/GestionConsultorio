using GestionConsultorio.Shared.Models;

namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface IHistorialClinicoRepository : IRepository<HistorialClinico>
{
    Task<IEnumerable<HistorialClinico>> ObtenerTodosConRelacionesAsync();
    Task<HistorialClinico?> ObtenerPorIdConRelacionesAsync(int id);
    Task<IEnumerable<HistorialClinico>> ObtenerPorPacienteAsync(int pacienteId);
    Task<HistorialClinico?> ObtenerPorTurnoAsync(int turnoId);
    Task<bool> ExisteHistorialParaTurnoAsync(int turnoId);
}