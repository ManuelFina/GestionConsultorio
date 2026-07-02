using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Medicos;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class MedicoService(
    IMedicoRepository medicoRepository,
    IAuthRepository authRepository,
    IRepository<Especialidad> especialidadRepository) : IMedicoService
{
    private readonly IMedicoRepository _medicoRepository = medicoRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IRepository<Especialidad> _especialidadRepository = especialidadRepository;

    public async Task<ResultadoOperacion<Medico>> RegistrarAsync(RegistroMedicoDto dto)
    {
        var especialidad = await _especialidadRepository.ObtenerPorIdAsync(dto.EspecialidadId);

        if (especialidad is null)
            return ResultadoOperacion<Medico>.Error("La especialidad seleccionada no existe.");

        var existeUsuario = await _authRepository.ExisteEmailAsync(dto.Email);

        if (existeUsuario)
            return ResultadoOperacion<Medico>.Error("Ya existe un usuario registrado con ese email.");

        var medicoConEmail = await _medicoRepository.ObtenerPorEmailAsync(dto.Email);

        if (medicoConEmail is not null)
            return ResultadoOperacion<Medico>.Error("Ya existe un médico registrado con ese email.");

        var existeMatricula = await _medicoRepository.ExisteMatriculaAsync(dto.Matricula);

        if (existeMatricula)
            return ResultadoOperacion<Medico>.Error("Ya existe un médico registrado con esa matrícula.");

        var medico = new Medico
        {
            NombreCompleto = dto.NombreCompleto,
            Matricula = dto.Matricula,
            Telefono = dto.Telefono,
            Email = dto.Email,
            EspecialidadId = dto.EspecialidadId
        };

        var usuario = new Usuario
        {
            NombreCompleto = dto.NombreCompleto,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = "Medico"
        };

        await _medicoRepository.CrearAsync(medico);
        await _authRepository.CrearUsuarioAsync(usuario);

        return ResultadoOperacion<Medico>.Ok(medico);
    }
}