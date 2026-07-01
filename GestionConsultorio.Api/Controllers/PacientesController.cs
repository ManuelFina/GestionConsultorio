using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize(Roles = "Administrador,Recepcionista")]
[ApiController]
[Route("api/[controller]")]
public class PacientesController(IPacienteRepository pacienteRepository) : ControllerBase
{
    private readonly IPacienteRepository _pacienteRepository = pacienteRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Paciente>>> ObtenerTodos()
    {
        var pacientes = await _pacienteRepository.ObtenerTodosAsync();
        return Ok(pacientes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Paciente>> ObtenerPorId(int id)
    {
        var paciente = await _pacienteRepository.ObtenerPorIdAsync(id);

        if (paciente is null)
            return NotFound("Paciente no encontrado.");

        return Ok(paciente);
    }

    [HttpGet("dni/{dni}")]
    public async Task<ActionResult<Paciente>> ObtenerPorDni(string dni)
    {
        var paciente = await _pacienteRepository.ObtenerPorDniAsync(dni);

        if (paciente is null)
            return NotFound("Paciente no encontrado.");

        return Ok(paciente);
    }

    [HttpPost]
    public async Task<ActionResult<Paciente>> Crear(Paciente paciente)
    {
        var existeDni = await _pacienteRepository.ExisteDniAsync(paciente.Dni);

        if (existeDni)
            return BadRequest("Ya existe un paciente registrado con ese DNI.");

        await _pacienteRepository.CrearAsync(paciente);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = paciente.Id },
            paciente
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Paciente paciente)
    {
        if (id != paciente.Id)
            return BadRequest("El ID de la ruta no coincide con el ID del paciente.");

        var pacienteExistente = await _pacienteRepository.ObtenerPorIdAsync(id);

        if (pacienteExistente is null)
            return NotFound("Paciente no encontrado.");

        var pacienteConMismoDni = await _pacienteRepository.ObtenerPorDniAsync(paciente.Dni);

        if (pacienteConMismoDni is not null && pacienteConMismoDni.Id != id)
            return BadRequest("Ya existe otro paciente registrado con ese DNI.");

        pacienteExistente.NombreCompleto = paciente.NombreCompleto;
        pacienteExistente.Dni = paciente.Dni;
        pacienteExistente.FechaNacimiento = paciente.FechaNacimiento;
        pacienteExistente.Telefono = paciente.Telefono;
        pacienteExistente.Email = paciente.Email;
        pacienteExistente.Direccion = paciente.Direccion;
        pacienteExistente.ObraSocial = paciente.ObraSocial;
        pacienteExistente.NumeroAfiliado = paciente.NumeroAfiliado;

        await _pacienteRepository.ActualizarAsync(pacienteExistente);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var paciente = await _pacienteRepository.ObtenerPorIdAsync(id);

        if (paciente is null)
            return NotFound("Paciente no encontrado.");

        await _pacienteRepository.EliminarAsync(paciente);

        return NoContent();
    }
}