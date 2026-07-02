using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize(Roles = "Administrador")]
[ApiController]
[Route("api/[controller]")]
public class MedicosController(
    IMedicoRepository medicoRepository,
    IValidacionEliminacionService validacionEliminacionService) : ControllerBase
{
    private readonly IMedicoRepository _medicoRepository = medicoRepository;
    private readonly IValidacionEliminacionService _validacionEliminacionService = validacionEliminacionService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Medico>>> ObtenerTodos()
    {
        var medicos = await _medicoRepository.ObtenerTodosConEspecialidadAsync();
        return Ok(medicos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Medico>> ObtenerPorId(int id)
    {
        var medico = await _medicoRepository.ObtenerPorIdConEspecialidadAsync(id);

        if (medico is null)
            return NotFound("Médico no encontrado.");

        return Ok(medico);
    }

    [HttpGet("matricula/{matricula}")]
    public async Task<ActionResult<Medico>> ObtenerPorMatricula(string matricula)
    {
        var medico = await _medicoRepository.ObtenerPorMatriculaAsync(matricula);

        if (medico is null)
            return NotFound("Médico no encontrado.");

        return Ok(medico);
    }

    [HttpGet("especialidad/{especialidadId:int}")]
    public async Task<ActionResult<IEnumerable<Medico>>> ObtenerPorEspecialidad(int especialidadId)
    {
        var medicos = await _medicoRepository.ObtenerPorEspecialidadAsync(especialidadId);
        return Ok(medicos);
    }

    [HttpPost]
    public async Task<ActionResult<Medico>> Crear(Medico medico)
    {
        var existeMatricula = await _medicoRepository.ExisteMatriculaAsync(medico.Matricula);

        if (existeMatricula)
            return BadRequest("Ya existe un médico registrado con esa matrícula.");

        await _medicoRepository.CrearAsync(medico);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = medico.Id },
            medico
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Medico medico)
    {
        if (id != medico.Id)
            return BadRequest("El ID de la ruta no coincide con el ID del médico.");

        var medicoExistente = await _medicoRepository.ObtenerPorIdAsync(id);

        if (medicoExistente is null)
            return NotFound("Médico no encontrado.");

        var medicoConMismaMatricula = await _medicoRepository.ObtenerPorMatriculaAsync(medico.Matricula);

        if (medicoConMismaMatricula is not null && medicoConMismaMatricula.Id != id)
            return BadRequest("Ya existe otro médico registrado con esa matrícula.");

        medicoExistente.NombreCompleto = medico.NombreCompleto;
        medicoExistente.Matricula = medico.Matricula;
        medicoExistente.Telefono = medico.Telefono;
        medicoExistente.Email = medico.Email;
        medicoExistente.EspecialidadId = medico.EspecialidadId;

        await _medicoRepository.ActualizarAsync(medicoExistente);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var medico = await _medicoRepository.ObtenerPorIdAsync(id);

        if (medico is null)
            return NotFound("Médico no encontrado.");

        var validacion = await _validacionEliminacionService.ValidarMedicoAsync(id);

        if (!validacion.PuedeEliminar)
            return BadRequest(validacion.Mensaje);

        await _medicoRepository.EliminarAsync(medico);

        return NoContent();
    }
}