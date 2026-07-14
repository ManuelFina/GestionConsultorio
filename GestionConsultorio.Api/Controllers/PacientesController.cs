using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PacientesController(
    IPacienteRepository pacienteRepository,
    IPacienteService pacienteService,
    IValidacionEliminacionService validacionEliminacionService) : ControllerBase
{
    private readonly IPacienteRepository _pacienteRepository = pacienteRepository;
    private readonly IPacienteService _pacienteService = pacienteService;
    private readonly IValidacionEliminacionService _validacionEliminacionService = validacionEliminacionService;

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Paciente>>> ObtenerTodos()
    {
        var pacientes = await _pacienteService.ObtenerTodosAsync();
        return Ok(pacientes);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Paciente>> ObtenerPorId(int id)
    {
        var paciente = await _pacienteService.ObtenerPorIdAsync(id);

        if (paciente is null)
            return NotFound("Paciente no encontrado.");

        return Ok(paciente);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("dni/{dni}")]
    public async Task<ActionResult<Paciente>> ObtenerPorDni(string dni)
    {
        var paciente = await _pacienteRepository.ObtenerPorDniAsync(dni.Trim());

        if (paciente is null)
            return NotFound("Paciente no encontrado.");

        return Ok(paciente);
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpPost]
    public async Task<ActionResult<Paciente>> Crear(Paciente paciente)
    {
        var resultado = await _pacienteService.CrearAsync(paciente);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = resultado.Data!.Id },
            resultado.Data
        );
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Paciente paciente)
    {
        var resultado = await _pacienteService.ActualizarAsync(id, paciente);

        if (!resultado.Exitoso)
        {
            if (resultado.Mensaje == "Paciente no encontrado.")
                return NotFound(resultado.Mensaje);

            return BadRequest(resultado.Mensaje);
        }

        return NoContent();
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var paciente = await _pacienteRepository.ObtenerPorIdAsync(id);

        if (paciente is null)
            return NotFound("Paciente no encontrado.");

        var validacion = await _validacionEliminacionService.ValidarPacienteAsync(id);

        if (!validacion.PuedeEliminar)
            return BadRequest(validacion.Mensaje);

        await _pacienteRepository.EliminarAsync(paciente);

        return NoContent();
    }
}