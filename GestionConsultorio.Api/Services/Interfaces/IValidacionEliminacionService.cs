namespace GestionConsultorio.Api.Services.Interfaces;

public interface IValidacionEliminacionService
{
    Task<ResultadoValidacionEliminacion> ValidarEspecialidadAsync(int especialidadId);
    Task<ResultadoValidacionEliminacion> ValidarMedicoAsync(int medicoId);
    Task<ResultadoValidacionEliminacion> ValidarPacienteAsync(int pacienteId);
    Task<ResultadoValidacionEliminacion> ValidarConsultorioAsync(int consultorioId);
    Task<ResultadoValidacionEliminacion> ValidarTurnoAsync(int turnoId);
}

public record ResultadoValidacionEliminacion(
    bool PuedeEliminar,
    string Mensaje = ""
);