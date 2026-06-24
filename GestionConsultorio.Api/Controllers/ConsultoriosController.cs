using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultoriosController(IRepository<Consultorio> consultorioRepository) : ControllerBase
{
    private readonly IRepository<Consultorio> _consultorioRepository = consultorioRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Consultorio>>> ObtenerTodos()
    {
        var consultorios = await _consultorioRepository.ObtenerTodosAsync();
        return Ok(consultorios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Consultorio>> ObtenerPorId(int id)
    {
        var consultorio = await _consultorioRepository.ObtenerPorIdAsync(id);

        if (consultorio is null)
            return NotFound("Consultorio no encontrado.");

        return Ok(consultorio);
    }

    [HttpPost]
    public async Task<ActionResult<Consultorio>> Crear(Consultorio consultorio)
    {
        await _consultorioRepository.CrearAsync(consultorio);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = consultorio.Id },
            consultorio
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Consultorio consultorio)
    {
        if (id != consultorio.Id)
            return BadRequest("El ID de la ruta no coincide con el ID del consultorio.");

        var consultorioExistente = await _consultorioRepository.ObtenerPorIdAsync(id);

        if (consultorioExistente is null)
            return NotFound("Consultorio no encontrado.");

        consultorioExistente.Nombre = consultorio.Nombre;

        await _consultorioRepository.ActualizarAsync(consultorioExistente);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var consultorio = await _consultorioRepository.ObtenerPorIdAsync(id);

        if (consultorio is null)
            return NotFound("Consultorio no encontrado.");

        await _consultorioRepository.EliminarAsync(consultorio);

        return NoContent();
    }
}