using GestionConsultorio.Shared.Models;

namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface IPacienteRepository : IRepository<Paciente>
{
    Task<IEnumerable<Paciente>> ObtenerActivosAsync();

    Task<IEnumerable<Paciente>> ObtenerInactivosAsync();

    Task<Paciente?> ObtenerActivoPorIdAsync(int id);

    Task<Paciente?> ObtenerPorDniAsync(string dni);

    Task<bool> ExisteDniAsync(string dni);

    Task<IEnumerable<Paciente>> ObtenerPorMedicoAsync(int medicoId);
}