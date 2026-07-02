using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Interfaces;

public interface IPacienteService
{
    Task<ResultadoOperacion<Paciente>> CrearAsync(Paciente paciente);
    Task<ResultadoOperacion<Paciente>> ActualizarAsync(int id, Paciente paciente);
}