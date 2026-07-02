using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Medicos;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class MedicoService(
    IMedicoRepository medicoRepository,
    IAuthRepository authRepository,
    IRepository<Especialidad> especialidadRepository,
    IValidacionEliminacionService validacionEliminacionService) : IMedicoService
{
    private readonly IMedicoRepository _medicoRepository = medicoRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IRepository<Especialidad> _especialidadRepository = especialidadRepository;
    private readonly IValidacionEliminacionService _validacionEliminacionService = validacionEliminacionService;

    public async Task<IEnumerable<Medico>> ObtenerTodosAsync()
    {
        return await _medicoRepository.ObtenerTodosConEspecialidadAsync();
    }

    public async Task<Medico?> ObtenerPorIdAsync(int id)
    {
        return await _medicoRepository.ObtenerPorIdConEspecialidadAsync(id);
    }

    public async Task<Medico?> ObtenerPorMatriculaAsync(string matricula)
    {
        return await _medicoRepository.ObtenerPorMatriculaAsync(matricula.Trim());
    }

    public async Task<IEnumerable<Medico>> ObtenerPorEspecialidadAsync(int especialidadId)
    {
        return await _medicoRepository.ObtenerPorEspecialidadAsync(especialidadId);
    }

    public async Task<ResultadoOperacion<Medico>> RegistrarAsync(RegistroMedicoDto dto)
    {
        NormalizarRegistro(dto);

        var validacion = await ValidarRegistroAsync(dto);

        if (!validacion.Exitoso)
            return ResultadoOperacion<Medico>.Error(validacion.Mensaje);

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

    public async Task<ResultadoOperacion<Medico>> ActualizarAsync(int id, Medico medico)
    {
        if (id != medico.Id)
            return ResultadoOperacion<Medico>.Error("El ID de la ruta no coincide con el ID del médico.");

        NormalizarMedico(medico);

        var validacion = await ValidarMedicoAsync(medico, id);

        if (!validacion.Exitoso)
            return ResultadoOperacion<Medico>.Error(validacion.Mensaje);

        var medicoExistente = await _medicoRepository.ObtenerPorIdAsync(id);

        if (medicoExistente is null)
            return ResultadoOperacion<Medico>.Error("Médico no encontrado.");

        medicoExistente.NombreCompleto = medico.NombreCompleto;
        medicoExistente.Matricula = medico.Matricula;
        medicoExistente.Telefono = medico.Telefono;
        medicoExistente.Email = medico.Email;
        medicoExistente.EspecialidadId = medico.EspecialidadId;

        await _medicoRepository.ActualizarAsync(medicoExistente);

        return ResultadoOperacion<Medico>.Ok(medicoExistente);
    }

    public async Task<ResultadoOperacion<bool>> EliminarAsync(int id)
    {
        var medico = await _medicoRepository.ObtenerPorIdAsync(id);

        if (medico is null)
            return ResultadoOperacion<bool>.Error("Médico no encontrado.");

        var validacion = await _validacionEliminacionService.ValidarMedicoAsync(id);

        if (!validacion.PuedeEliminar)
            return ResultadoOperacion<bool>.Error(validacion.Mensaje);

        await _medicoRepository.EliminarAsync(medico);

        return ResultadoOperacion<bool>.Ok(true);
    }

    private async Task<ResultadoOperacion<bool>> ValidarRegistroAsync(RegistroMedicoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NombreCompleto))
            return ResultadoOperacion<bool>.Error("El nombre completo del médico es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Matricula))
            return ResultadoOperacion<bool>.Error("La matrícula del médico es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.Telefono))
            return ResultadoOperacion<bool>.Error("El teléfono del médico es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return ResultadoOperacion<bool>.Error("El email del médico es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            return ResultadoOperacion<bool>.Error("La contraseña es obligatoria.");

        if (dto.Password.Length < 6)
            return ResultadoOperacion<bool>.Error("La contraseña debe tener al menos 6 caracteres.");

        var especialidad = await _especialidadRepository.ObtenerPorIdAsync(dto.EspecialidadId);

        if (especialidad is null)
            return ResultadoOperacion<bool>.Error("La especialidad seleccionada no existe.");

        var existeUsuario = await _authRepository.ExisteEmailAsync(dto.Email);

        if (existeUsuario)
            return ResultadoOperacion<bool>.Error("Ya existe un usuario registrado con ese email.");

        var medicoConEmail = await _medicoRepository.ObtenerPorEmailAsync(dto.Email);

        if (medicoConEmail is not null)
            return ResultadoOperacion<bool>.Error("Ya existe un médico registrado con ese email.");

        var existeMatricula = await _medicoRepository.ExisteMatriculaAsync(dto.Matricula);

        if (existeMatricula)
            return ResultadoOperacion<bool>.Error("Ya existe un médico registrado con esa matrícula.");

        return ResultadoOperacion<bool>.Ok(true);
    }

    private async Task<ResultadoOperacion<bool>> ValidarMedicoAsync(Medico medico, int medicoIdExcluir)
    {
        if (string.IsNullOrWhiteSpace(medico.NombreCompleto))
            return ResultadoOperacion<bool>.Error("El nombre completo del médico es obligatorio.");

        if (string.IsNullOrWhiteSpace(medico.Matricula))
            return ResultadoOperacion<bool>.Error("La matrícula del médico es obligatoria.");

        if (string.IsNullOrWhiteSpace(medico.Telefono))
            return ResultadoOperacion<bool>.Error("El teléfono del médico es obligatorio.");

        if (string.IsNullOrWhiteSpace(medico.Email))
            return ResultadoOperacion<bool>.Error("El email del médico es obligatorio.");

        var especialidad = await _especialidadRepository.ObtenerPorIdAsync(medico.EspecialidadId);

        if (especialidad is null)
            return ResultadoOperacion<bool>.Error("La especialidad seleccionada no existe.");

        var medicoConMismaMatricula = await _medicoRepository.ObtenerPorMatriculaAsync(medico.Matricula);

        if (medicoConMismaMatricula is not null && medicoConMismaMatricula.Id != medicoIdExcluir)
            return ResultadoOperacion<bool>.Error("Ya existe otro médico registrado con esa matrícula.");

        var medicoConMismoEmail = await _medicoRepository.ObtenerPorEmailAsync(medico.Email);

        if (medicoConMismoEmail is not null && medicoConMismoEmail.Id != medicoIdExcluir)
            return ResultadoOperacion<bool>.Error("Ya existe otro médico registrado con ese email.");

        return ResultadoOperacion<bool>.Ok(true);
    }

    private static void NormalizarRegistro(RegistroMedicoDto dto)
    {
        dto.NombreCompleto = dto.NombreCompleto?.Trim() ?? string.Empty;
        dto.Matricula = dto.Matricula?.Trim() ?? string.Empty;
        dto.Telefono = dto.Telefono?.Trim() ?? string.Empty;
        dto.Email = dto.Email?.Trim() ?? string.Empty;
    }

    private static void NormalizarMedico(Medico medico)
    {
        medico.NombreCompleto = medico.NombreCompleto?.Trim() ?? string.Empty;
        medico.Matricula = medico.Matricula?.Trim() ?? string.Empty;
        medico.Telefono = medico.Telefono?.Trim() ?? string.Empty;
        medico.Email = medico.Email?.Trim() ?? string.Empty;
    }
}