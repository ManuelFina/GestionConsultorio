using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize(Roles = "Administrador")]
[ApiController]
[Route("api/[controller]")]
public class EspecialidadesController(IRepository<Especialidad> especialidadRepository) : ControllerBase
{
    private readonly IRepository<Especialidad> _especialidadRepository = especialidadRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Especialidad>>> ObtenerTodos()
    {
        var especialidades = await _especialidadRepository.ObtenerTodosAsync();
        return Ok(especialidades);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Especialidad>> ObtenerPorId(int id)
    {
        var especialidad = await _especialidadRepository.ObtenerPorIdAsync(id);

        if (especialidad is null)
            return NotFound("Especialidad no encontrada.");

        return Ok(especialidad);
    }

    [HttpPost]
    public async Task<ActionResult<Especialidad>> Crear(Especialidad especialidad)
    {
        await _especialidadRepository.CrearAsync(especialidad);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = especialidad.Id },
            especialidad
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Especialidad especialidad)
    {
        if (id != especialidad.Id)
            return BadRequest("El ID de la ruta no coincide con el ID de la especialidad.");

        var especialidadExistente = await _especialidadRepository.ObtenerPorIdAsync(id);

        if (especialidadExistente is null)
            return NotFound("Especialidad no encontrada.");

        especialidadExistente.Nombre = especialidad.Nombre;

        await _especialidadRepository.ActualizarAsync(especialidadExistente);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var especialidad = await _especialidadRepository.ObtenerPorIdAsync(id);

        if (especialidad is null)
            return NotFound("Especialidad no encontrada.");

        await _especialidadRepository.EliminarAsync(especialidad);

        return NoContent();
    }
}