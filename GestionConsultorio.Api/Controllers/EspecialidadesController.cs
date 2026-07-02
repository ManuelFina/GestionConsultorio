using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EspecialidadesController(
    IRepository<Especialidad> especialidadRepository,
    IValidacionEliminacionService validacionEliminacionService) : ControllerBase
{
    private readonly IRepository<Especialidad> _especialidadRepository = especialidadRepository;
    private readonly IValidacionEliminacionService _validacionEliminacionService = validacionEliminacionService;

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Especialidad>>> ObtenerTodos()
    {
        var especialidades = await _especialidadRepository.ObtenerTodosAsync();
        return Ok(especialidades);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Especialidad>> ObtenerPorId(int id)
    {
        var especialidad = await _especialidadRepository.ObtenerPorIdAsync(id);

        if (especialidad is null)
            return NotFound("Especialidad no encontrada.");

        return Ok(especialidad);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<ActionResult<Especialidad>> Crear(Especialidad especialidad)
    {
        var nombreNormalizado = especialidad.Nombre.Trim();

        if (string.IsNullOrWhiteSpace(nombreNormalizado))
            return BadRequest("El nombre de la especialidad es obligatorio.");

        var especialidades = await _especialidadRepository.ObtenerTodosAsync();

        var existeEspecialidad = especialidades.Any(e =>
            e.Nombre.Trim().Equals(nombreNormalizado, StringComparison.OrdinalIgnoreCase)
        );

        if (existeEspecialidad)
            return BadRequest("Ya existe una especialidad registrada con ese nombre.");

        especialidad.Nombre = nombreNormalizado;

        await _especialidadRepository.CrearAsync(especialidad);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = especialidad.Id },
            especialidad
        );
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Especialidad especialidad)
    {
        if (id != especialidad.Id)
            return BadRequest("El ID de la ruta no coincide con el ID de la especialidad.");

        var especialidadExistente = await _especialidadRepository.ObtenerPorIdAsync(id);

        if (especialidadExistente is null)
            return NotFound("Especialidad no encontrada.");

        var nombreNormalizado = especialidad.Nombre.Trim();

        if (string.IsNullOrWhiteSpace(nombreNormalizado))
            return BadRequest("El nombre de la especialidad es obligatorio.");

        var especialidades = await _especialidadRepository.ObtenerTodosAsync();

        var existeOtraEspecialidad = especialidades.Any(e =>
            e.Id != id &&
            e.Nombre.Trim().Equals(nombreNormalizado, StringComparison.OrdinalIgnoreCase)
        );

        if (existeOtraEspecialidad)
            return BadRequest("Ya existe otra especialidad registrada con ese nombre.");

        especialidadExistente.Nombre = nombreNormalizado;

        await _especialidadRepository.ActualizarAsync(especialidadExistente);

        return NoContent();
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var especialidad = await _especialidadRepository.ObtenerPorIdAsync(id);

        if (especialidad is null)
            return NotFound("Especialidad no encontrada.");

        var validacion = await _validacionEliminacionService.ValidarEspecialidadAsync(id);

        if (!validacion.PuedeEliminar)
            return BadRequest(validacion.Mensaje);

        await _especialidadRepository.EliminarAsync(especialidad);

        return NoContent();
    }
}