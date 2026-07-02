using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Medicos;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicosController(
    IMedicoRepository medicoRepository,
    IValidacionEliminacionService validacionEliminacionService,
    IMedicoService medicoService) : ControllerBase
{
    private readonly IMedicoRepository _medicoRepository = medicoRepository;
    private readonly IValidacionEliminacionService _validacionEliminacionService = validacionEliminacionService;
    private readonly IMedicoService _medicoService = medicoService;

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Medico>>> ObtenerTodos()
    {
        var medicos = await _medicoRepository.ObtenerTodosConEspecialidadAsync();
        return Ok(medicos);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Medico>> ObtenerPorId(int id)
    {
        var medico = await _medicoRepository.ObtenerPorIdConEspecialidadAsync(id);

        if (medico is null)
            return NotFound("Médico no encontrado.");

        return Ok(medico);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("matricula/{matricula}")]
    public async Task<ActionResult<Medico>> ObtenerPorMatricula(string matricula)
    {
        var medico = await _medicoRepository.ObtenerPorMatriculaAsync(matricula);

        if (medico is null)
            return NotFound("Médico no encontrado.");

        return Ok(medico);
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("especialidad/{especialidadId:int}")]
    public async Task<ActionResult<IEnumerable<Medico>>> ObtenerPorEspecialidad(int especialidadId)
    {
        var medicos = await _medicoRepository.ObtenerPorEspecialidadAsync(especialidadId);
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

    [Authorize(Roles = "Administrador")]
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