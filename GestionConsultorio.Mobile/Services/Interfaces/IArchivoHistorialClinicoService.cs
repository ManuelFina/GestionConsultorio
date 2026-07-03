using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;
using Microsoft.AspNetCore.Components.Forms;

namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface IArchivoHistorialClinicoService
{
    Task<ApiResponse<List<ArchivoHistorialClinico>>> ObtenerPorHistorialAsync(int historialClinicoId);
    Task<ApiResponse<ArchivoHistorialClinico>> SubirArchivoAsync(int historialClinicoId, IBrowserFile archivo);
    Task<ApiResponse<bool>> EliminarAsync(int archivoId);
}