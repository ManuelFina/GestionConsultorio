using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Medicos;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicosController(IMedicoService medicoService) : ControllerBase
{
    private readonly IMedicoService _medicoService = medicoService;

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Medico>>> ObtenerTodos()
    {
        var medicos = await _medicoService.ObtenerTodosAsync();
        return Ok(medicos);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Medico>> ObtenerPorId(int id)
    {
        var medico = await _medicoService.ObtenerPorIdAsync(id);

        if (medico is null)
            return NotFound("Médico no encontrado.");

        return Ok(medico);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("matricula/{matricula}")]
    public async Task<ActionResult<Medico>> ObtenerPorMatricula(string matricula)
    {
        var medico = await _medicoService.ObtenerPorMatriculaAsync(matricula);

        if (medico is null)
            return NotFound("Médico no encontrado.");

        return Ok(medico);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("especialidad/{especialidadId:int}")]
    public async Task<ActionResult<IEnumerable<Medico>>> ObtenerPorEspecialidad(int especialidadId)
    {
        var medicos = await _medicoService.ObtenerPorEspecialidadAsync(especialidadId);
        return Ok(medicos);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<Medico>> Registrar(RegistroMedicoDto dto)
    {
        var resultado = await _medicoService.RegistrarAsync(dto);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = resultado.Data!.Id },
            resultado.Data
        );
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Medico medico)
    {
        var resultado = await _medicoService.ActualizarAsync(id, medico);

        if (resultado.Exitoso)
            return NoContent();

        if (resultado.Mensaje == "Médico no encontrado.")
            return NotFound(resultado.Mensaje);

        return BadRequest(resultado.Mensaje);
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _medicoService.EliminarAsync(id);

        if (resultado.Exitoso)
            return NoContent();

        if (resultado.Mensaje == "Médico no encontrado.")
            return NotFound(resultado.Mensaje);

        return BadRequest(resultado.Mensaje);
    }
}