using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Auth;
using GestionConsultorio.Shared.DTOs.Recepcionistas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize(Roles = "Administrador")]
[ApiController]
[Route("api/[controller]")]
public class RecepcionistasController(IRecepcionistaService recepcionistaService) : ControllerBase
{
    private readonly IRecepcionistaService _recepcionistaService = recepcionistaService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecepcionistaDto>>> ObtenerTodos()
    {
        var resultado = await _recepcionistaService.ObtenerTodosAsync();

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return Ok(resultado.Data);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RecepcionistaDto>> ObtenerPorId(int id)
    {
        var resultado = await _recepcionistaService.ObtenerPorIdAsync(id);

        if (!resultado.Exitoso)
            return NotFound(resultado.Mensaje);

        return Ok(resultado.Data);
    }

    [HttpPost]
    public async Task<ActionResult<RecepcionistaDto>> Crear(RegistroUsuarioDto dto)
    {
        var resultado = await _recepcionistaService.CrearAsync(dto);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = resultado.Data!.Id },
            resultado.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RecepcionistaDto>> Actualizar(int id, ActualizarRecepcionistaDto dto)
    {
        var resultado = await _recepcionistaService.ActualizarAsync(id, dto);

        if (!resultado.Exitoso)
        {
            if (resultado.Mensaje == "Recepcionista no encontrada.")
                return NotFound(resultado.Mensaje);

            return BadRequest(resultado.Mensaje);
        }

        return Ok(resultado.Data);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _recepcionistaService.EliminarAsync(id);

        if (!resultado.Exitoso)
            return NotFound(resultado.Mensaje);

        return NoContent();
    }
}