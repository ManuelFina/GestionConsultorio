using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/historialesclinicos")]
public class ArchivosHistorialClinicoController(
    IArchivoHistorialClinicoService archivoService) : ControllerBase
{
    private readonly IArchivoHistorialClinicoService _archivoService = archivoService;

    [Authorize(Roles = "Administrador,Medico")]
    [HttpGet("{historialClinicoId:int}/archivos")]
    public async Task<ActionResult<IEnumerable<ArchivoHistorialClinico>>> ObtenerPorHistorial(
        int historialClinicoId)
    {
        var archivos = await _archivoService.ObtenerPorHistorialAsync(historialClinicoId);
        return Ok(archivos);
    }

    [Authorize(Roles = "Administrador,Medico")]
    [HttpPost("{historialClinicoId:int}/archivos")]
    public async Task<ActionResult<ArchivoHistorialClinico>> SubirArchivo(
        int historialClinicoId,
        [FromForm] IFormFile archivo)
    {
        var resultado = await _archivoService.SubirArchivoAsync(historialClinicoId, archivo);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return Ok(resultado.Data);
    }

    [Authorize(Roles = "Administrador,Medico")]
    [HttpDelete("archivos/{archivoId:int}")]
    public async Task<IActionResult> EliminarArchivo(int archivoId)
    {
        var resultado = await _archivoService.EliminarAsync(archivoId);

        if (resultado.Exitoso)
            return NoContent();

        if (resultado.Mensaje == "Archivo no encontrado.")
            return NotFound(resultado.Mensaje);

        return BadRequest(resultado.Mensaje);
    }
}