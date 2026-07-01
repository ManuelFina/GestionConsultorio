using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Enums;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TurnosController(ITurnoService turnoService) : ControllerBase
{
    private readonly ITurnoService _turnoService = turnoService;

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Turno>>> ObtenerTodos()
    {
        var turnos = await _turnoService.ObtenerTodosAsync();
        return Ok(turnos);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Turno>> ObtenerPorId(int id)
    {
        var turno = await _turnoService.ObtenerPorIdAsync(id);

        if (turno is null)
            return NotFound("Turno no encontrado.");

        return Ok(turno);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("fecha/{fecha}")]
    public async Task<ActionResult<IEnumerable<Turno>>> ObtenerPorFecha(DateOnly fecha)
    {
        var turnos = await _turnoService.ObtenerPorFechaAsync(fecha);
        return Ok(turnos);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("medico/{medicoId:int}")]
    public async Task<ActionResult<IEnumerable<Turno>>> ObtenerPorMedico(int medicoId)
    {
        var turnos = await _turnoService.ObtenerPorMedicoAsync(medicoId);
        return Ok(turnos);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("paciente/{pacienteId:int}")]
    public async Task<ActionResult<IEnumerable<Turno>>> ObtenerPorPaciente(int pacienteId)
    {
        var turnos = await _turnoService.ObtenerPorPacienteAsync(pacienteId);
        return Ok(turnos);
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpPost]
    public async Task<ActionResult<Turno>> Crear(Turno turno)
    {
        var resultado = await _turnoService.CrearAsync(turno);

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
    public async Task<IActionResult> Actualizar(int id, Turno turno)
    {
        var resultado = await _turnoService.ActualizarAsync(id, turno);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return NoContent();
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpPatch("{id:int}/confirmar")]
    public async Task<IActionResult> Confirmar(int id)
    {
        var resultado = await _turnoService.CambiarEstadoAsync(id, EstadoTurno.Confirmado);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return NoContent();
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpPatch("{id:int}/cancelar")]
    public async Task<IActionResult> Cancelar(int id)
    {
        var resultado = await _turnoService.CambiarEstadoAsync(id, EstadoTurno.Cancelado);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return NoContent();
    }

    [Authorize(Roles = "Administrador,Medico")]
    [HttpPatch("{id:int}/atender")]
    public async Task<IActionResult> Atender(int id)
    {
        var resultado = await _turnoService.CambiarEstadoAsync(id, EstadoTurno.Atendido);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return NoContent();
    }

    [Authorize(Roles = "Administrador,Medico")]
    [HttpPatch("{id:int}/ausente")]
    public async Task<IActionResult> MarcarAusente(int id)
    {
        var resultado = await _turnoService.CambiarEstadoAsync(id, EstadoTurno.Ausente);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return NoContent();
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _turnoService.EliminarAsync(id);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return NoContent();
    }
}