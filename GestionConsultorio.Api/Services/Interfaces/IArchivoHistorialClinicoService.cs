using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace GestionConsultorio.Api.Services.Interfaces;

public interface IArchivoHistorialClinicoService
{
    Task<IEnumerable<ArchivoHistorialClinico>> ObtenerPorHistorialAsync(int historialClinicoId);
    Task<ResultadoOperacion<ArchivoHistorialClinico>> SubirArchivoAsync(int historialClinicoId, IFormFile archivo);
    Task<ResultadoOperacion<bool>> EliminarAsync(int id);
}