using GestionConsultorio.Shared.Models;

namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface IMedicoRepository : IRepository<Medico>
{
    Task<IEnumerable<Medico>> ObtenerTodosConEspecialidadAsync();
    Task<Medico?> ObtenerPorIdConEspecialidadAsync(int id);
    Task<Medico?> ObtenerPorMatriculaAsync(string matricula);
    Task<bool> ExisteMatriculaAsync(string matricula);
    Task<IEnumerable<Medico>> ObtenerPorEspecialidadAsync(int especialidadId);
    Task<Medico?> ObtenerPorEmailAsync(string email);
}