using GestionConsultorio.Shared.DTOs.Medicos;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Interfaces;

public interface IMedicoService
{
    Task<ResultadoOperacion<Medico>> RegistrarAsync(RegistroMedicoDto dto);
}