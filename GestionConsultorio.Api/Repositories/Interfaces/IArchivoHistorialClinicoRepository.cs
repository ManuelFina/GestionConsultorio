using GestionConsultorio.Shared.Models;

namespace GestionConsultorio.Api.Repositories.Interfaces;

public interface IArchivoHistorialClinicoRepository : IRepository<ArchivoHistorialClinico>
{
    Task<IEnumerable<ArchivoHistorialClinico>> ObtenerPorHistorialAsync(int historialClinicoId);
    Task<ArchivoHistorialClinico?> ObtenerPorIdConHistorialAsync(int id);
}