using System.Security.Claims;
using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class PacienteService(
    IPacienteRepository pacienteRepository,
    IMedicoRepository medicoRepository,
    IHttpContextAccessor httpContextAccessor) : IPacienteService
{
    private readonly IPacienteRepository _pacienteRepository = pacienteRepository;
    private readonly IMedicoRepository _medicoRepository = medicoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<IEnumerable<Paciente>> ObtenerTodosAsync()
    {
        if (UsuarioEsMedico())
        {
            var medico = await ObtenerMedicoLogueadoAsync();

            if (medico is null)
                return Enumerable.Empty<Paciente>();

            return await _pacienteRepository.ObtenerPorMedicoAsync(medico.Id);
        }

        return await _pacienteRepository.ObtenerActivosAsync();
    }
    public async Task<IEnumerable<Paciente>> ObtenerInactivosAsync()
    {
        return await _pacienteRepository.ObtenerInactivosAsync();
    }
    public async Task<Paciente?> ObtenerPorIdAsync(int id)
    {
        var paciente = await _pacienteRepository.ObtenerActivoPorIdAsync(id);

        if (paciente is null)
            return null;

        if (UsuarioEsMedico())
        {
            var medico = await ObtenerMedicoLogueadoAsync();

            if (medico is null)
                return null;

            var pacientesDelMedico = await _pacienteRepository.ObtenerPorMedicoAsync(medico.Id);

            var perteneceAlMedico = pacientesDelMedico.Any(p => p.Id == id);

            if (!perteneceAlMedico)
                return null;
        }

        return paciente;
    }

    public async Task<ResultadoOperacion<Paciente>> CrearAsync(Paciente paciente)
    {
        NormalizarPaciente(paciente);

        var validacion = ValidarPaciente(paciente);

        if (!validacion.Exitoso)
            return ResultadoOperacion<Paciente>.Error(validacion.Mensaje);

        var existeDni = await _pacienteRepository.ExisteDniAsync(paciente.Dni);

        if (existeDni)
            return ResultadoOperacion<Paciente>.Error("Ya existe un paciente registrado con ese DNI.");

        paciente.Activo = true;
        paciente.FechaBaja = null;

        await _pacienteRepository.CrearAsync(paciente);

        return ResultadoOperacion<Paciente>.Ok(paciente);
    }

    public async Task<ResultadoOperacion<Paciente>> ActualizarAsync(int id, Paciente paciente)
    {
        if (id != paciente.Id)
            return ResultadoOperacion<Paciente>.Error("El ID de la ruta no coincide con el ID del paciente.");

        NormalizarPaciente(paciente);

        var validacion = ValidarPaciente(paciente);

        if (!validacion.Exitoso)
            return ResultadoOperacion<Paciente>.Error(validacion.Mensaje);

        var pacienteExistente = await _pacienteRepository.ObtenerPorIdAsync(id);

        if (pacienteExistente is null)
            return ResultadoOperacion<Paciente>.Error("Paciente no encontrado.");

        if (!pacienteExistente.Activo)
            return ResultadoOperacion<Paciente>.Error("No se puede modificar un paciente dado de baja.");

        var pacienteConMismoDni = await _pacienteRepository.ObtenerPorDniAsync(paciente.Dni);

        if (pacienteConMismoDni is not null && pacienteConMismoDni.Id != id)
            return ResultadoOperacion<Paciente>.Error("Ya existe otro paciente registrado con ese DNI.");

        pacienteExistente.NombreCompleto = paciente.NombreCompleto;
        pacienteExistente.Dni = paciente.Dni;
        pacienteExistente.FechaNacimiento = paciente.FechaNacimiento;
        pacienteExistente.Telefono = paciente.Telefono;
        pacienteExistente.Email = paciente.Email;
        pacienteExistente.Direccion = paciente.Direccion;
        pacienteExistente.ObraSocial = paciente.ObraSocial;
        pacienteExistente.NumeroAfiliado = paciente.NumeroAfiliado;

        await _pacienteRepository.ActualizarAsync(pacienteExistente);

        return ResultadoOperacion<Paciente>.Ok(pacienteExistente);
    }

    public async Task<ResultadoOperacion<bool>> EliminarAsync(int id)
    {
        var paciente = await _pacienteRepository.ObtenerPorIdAsync(id);

        if (paciente is null)
            return ResultadoOperacion<bool>.Error("Paciente no encontrado.");

        if (!paciente.Activo)
            return ResultadoOperacion<bool>.Error("El paciente ya se encuentra dado de baja.");

        paciente.Activo = false;
        paciente.FechaBaja = DateTime.Now;

        await _pacienteRepository.ActualizarAsync(paciente);

        return ResultadoOperacion<bool>.Ok(true);
    }

    public async Task<ResultadoOperacion<bool>> ReactivarAsync(int id)
    {
        var paciente = await _pacienteRepository.ObtenerPorIdAsync(id);

        if (paciente is null)
            return ResultadoOperacion<bool>.Error("Paciente no encontrado.");

        if (paciente.Activo)
            return ResultadoOperacion<bool>.Error("El paciente ya se encuentra activo.");

        paciente.Activo = true;
        paciente.FechaBaja = null;

        await _pacienteRepository.ActualizarAsync(paciente);

        return ResultadoOperacion<bool>.Ok(true);
    }

    private bool UsuarioEsMedico()
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole("Medico") == true;
    }

    private async Task<Medico?> ObtenerMedicoLogueadoAsync()
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _medicoRepository.ObtenerPorEmailAsync(email);
    }

    private static ResultadoOperacion<bool> ValidarPaciente(Paciente paciente)
    {
        if (string.IsNullOrWhiteSpace(paciente.NombreCompleto))
            return ResultadoOperacion<bool>.Error("El nombre completo del paciente es obligatorio.");

        if (string.IsNullOrWhiteSpace(paciente.Dni))
            return ResultadoOperacion<bool>.Error("El DNI del paciente es obligatorio.");

        if (paciente.FechaNacimiento.Date > DateTime.Today)
            return ResultadoOperacion<bool>.Error("La fecha de nacimiento no puede ser futura.");

        return ResultadoOperacion<bool>.Ok(true);
    }

    private static void NormalizarPaciente(Paciente paciente)
    {
        paciente.NombreCompleto = paciente.NombreCompleto?.Trim() ?? string.Empty;
        paciente.Dni = paciente.Dni?.Trim() ?? string.Empty;
        paciente.Telefono = paciente.Telefono?.Trim() ?? string.Empty;
        paciente.Email = paciente.Email?.Trim() ?? string.Empty;
        paciente.Direccion = paciente.Direccion?.Trim() ?? string.Empty;
        paciente.ObraSocial = paciente.ObraSocial?.Trim() ?? string.Empty;
        paciente.NumeroAfiliado = paciente.NumeroAfiliado?.Trim() ?? string.Empty;
    }
}