using GestionConsultorio.Shared.DTOs.HistorialesClinicos;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface IHistorialClinicoService
{
    Task<ApiResponse<List<HistorialClinico>>> ObtenerTodosAsync();
    Task<ApiResponse<HistorialClinico>> ObtenerPorIdAsync(int id);
    Task<ApiResponse<List<HistorialClinico>>> ObtenerPorPacienteAsync(int pacienteId);
    Task<ApiResponse<HistorialClinico>> ObtenerPorTurnoAsync(int turnoId);

    Task<ApiResponse<HistorialClinico>> CrearAsync(HistorialClinico historial);
    Task<ApiResponse<HistorialClinico>> AtenderTurnoAsync(AtenderTurnoDto dto);
    Task<ApiResponse<bool>> ActualizarAsync(int id, HistorialClinico historial);
    Task<ApiResponse<bool>> EliminarAsync(int id);
}