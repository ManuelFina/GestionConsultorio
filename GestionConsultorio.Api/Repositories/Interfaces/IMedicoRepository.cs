using GestionConsultorio.Shared.Models;

namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface IMedicoRepository : IRepository<Medico>
{
    Task<Medico?> ObtenerPorMatriculaAsync(string matricula);
    Task<bool> ExisteMatriculaAsync(string matricula);
    Task<IEnumerable<Medico>> ObtenerPorEspecialidadAsync(int especialidadId);
    Task<IEnumerable<Medico>> ObtenerTodosConEspecialidadAsync();
    Task<Medico?> ObtenerPorIdConEspecialidadAsync(int id);
}