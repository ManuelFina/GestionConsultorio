using GestionConsultorio.Shared.Models;

namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface IPacienteRepository : IRepository<Paciente>
{
    Task<Paciente?> ObtenerPorDniAsync(string dni);
    Task<bool> ExisteDniAsync(string dni);

    Task<IEnumerable<Paciente>> ObtenerPorMedicoAsync(int medicoId);


}