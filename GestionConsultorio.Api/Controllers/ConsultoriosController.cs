using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ConsultoriosController(
    IRepository<Consultorio> consultorioRepository,
    IValidacionEliminacionService validacionEliminacionService) : ControllerBase
{
    private readonly IRepository<Consultorio> _consultorioRepository = consultorioRepository;
    private readonly IValidacionEliminacionService _validacionEliminacionService = validacionEliminacionService;

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Consultorio>>> ObtenerTodos()
    {
        var consultorios = await _consultorioRepository.ObtenerTodosAsync();
        return Ok(consultorios);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Consultorio>> ObtenerPorId(int id)
    {
        var consultorio = await _consultorioRepository.ObtenerPorIdAsync(id);

        if (consultorio is null)
            return NotFound("Consultorio no encontrado.");

        return Ok(consultorio);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<Consultorio>> Crear(Consultorio consultorio)
    {
        var nombreNormalizado = consultorio.Nombre.Trim();

        if (string.IsNullOrWhiteSpace(nombreNormalizado))
            return BadRequest("El nombre del consultorio es obligatorio.");

        var consultorios = await _consultorioRepository.ObtenerTodosAsync();

        var existeConsultorio = consultorios.Any(c =>
            c.Nombre.Trim().Equals(nombreNormalizado, StringComparison.OrdinalIgnoreCase)
        );

        if (existeConsultorio)
            return BadRequest("Ya existe un consultorio registrado con ese nombre.");

        consultorio.Nombre = nombreNormalizado;

        await _consultorioRepository.CrearAsync(consultorio);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = consultorio.Id },
            consultorio
        );
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Consultorio consultorio)
    {
        if (id != consultorio.Id)
            return BadRequest("El ID de la ruta no coincide con el ID del consultorio.");

        var consultorioExistente = await _consultorioRepository.ObtenerPorIdAsync(id);

        if (consultorioExistente is null)
            return NotFound("Consultorio no encontrado.");

        var nombreNormalizado = consultorio.Nombre.Trim();

        if (string.IsNullOrWhiteSpace(nombreNormalizado))
            return BadRequest("El nombre del consultorio es obligatorio.");

        var consultorios = await _consultorioRepository.ObtenerTodosAsync();

        var existeOtroConsultorio = consultorios.Any(c =>
            c.Id != id &&
            c.Nombre.Trim().Equals(nombreNormalizado, StringComparison.OrdinalIgnoreCase)
        );

        if (existeOtroConsultorio)
            return BadRequest("Ya existe otro consultorio registrado con ese nombre.");

        consultorioExistente.Nombre = nombreNormalizado;

        await _consultorioRepository.ActualizarAsync(consultorioExistente);

        return NoContent();
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var consultorio = await _consultorioRepository.ObtenerPorIdAsync(id);

        if (consultorio is null)
            return NotFound("Consultorio no encontrado.");

        var validacion = await _validacionEliminacionService.ValidarConsultorioAsync(id);

        if (!validacion.PuedeEliminar)
            return BadRequest(validacion.Mensaje);

        await _consultorioRepository.EliminarAsync(consultorio);

        return NoContent();
    }
}