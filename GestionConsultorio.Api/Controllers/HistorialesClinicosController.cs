using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize(Roles = "Administrador,Medico")]
[ApiController]
[Route("api/[controller]")]
public class HistorialesClinicosController(IHistorialClinicoService historialClinicoService) : ControllerBase
{
    private readonly IHistorialClinicoService _historialClinicoService = historialClinicoService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HistorialClinico>>> ObtenerTodos()
    {
        var historiales = await _historialClinicoService.ObtenerTodosAsync();
        return Ok(historiales);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HistorialClinico>> ObtenerPorId(int id)
    {
        var historial = await _historialClinicoService.ObtenerPorIdAsync(id);

        if (historial is null)
            return NotFound("Historial clínico no encontrado.");

        return Ok(historial);
    }

    [HttpGet("paciente/{pacienteId:int}")]
    public async Task<ActionResult<IEnumerable<HistorialClinico>>> ObtenerPorPaciente(int pacienteId)
    {
        var historiales = await _historialClinicoService.ObtenerPorPacienteAsync(pacienteId);
        return Ok(historiales);
    }

    [HttpGet("turno/{turnoId:int}")]
    public async Task<ActionResult<HistorialClinico>> ObtenerPorTurno(int turnoId)
    {
        var historial = await _historialClinicoService.ObtenerPorTurnoAsync(turnoId);

        if (historial is null)
            return NotFound("Historial clínico no encontrado para ese turno.");

        return Ok(historial);
    }

    [HttpPost]
    public async Task<ActionResult<HistorialClinico>> Crear(HistorialClinico historial)
    {
        var resultado = await _historialClinicoService.CrearAsync(historial);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = resultado.Data!.Id },
            resultado.Data
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, HistorialClinico historial)
    {
        var resultado = await _historialClinicoService.ActualizarAsync(id, historial);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _historialClinicoService.EliminarAsync(id);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return NoContent();
    }
}