using GestionConsultorio.Shared.DTOs.Medicos;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface IMedicoService
{
    Task<ApiResponse<List<Medico>>> ObtenerTodosAsync();
    Task<ApiResponse<Medico>> ObtenerPorIdAsync(int id);
    Task<ApiResponse<Medico>> ObtenerPorMatriculaAsync(string matricula);
    Task<ApiResponse<List<Medico>>> ObtenerPorEspecialidadAsync(int especialidadId);

    Task<ApiResponse<Medico>> RegistrarAsync(RegistroMedicoDto dto);
    Task<ApiResponse<bool>> ActualizarAsync(int id, Medico medico);
    Task<ApiResponse<bool>> EliminarAsync(int id);
}