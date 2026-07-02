using GestionConsultorio.Shared.DTOs.Medicos;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Interfaces;

public interface IMedicoService
{
    Task<IEnumerable<Medico>> ObtenerTodosAsync();
    Task<Medico?> ObtenerPorIdAsync(int id);
    Task<Medico?> ObtenerPorMatriculaAsync(string matricula);
    Task<IEnumerable<Medico>> ObtenerPorEspecialidadAsync(int especialidadId);

    Task<ResultadoOperacion<Medico>> RegistrarAsync(RegistroMedicoDto dto);
    Task<ResultadoOperacion<Medico>> ActualizarAsync(int id, Medico medico);
    Task<ResultadoOperacion<bool>> EliminarAsync(int id);
}