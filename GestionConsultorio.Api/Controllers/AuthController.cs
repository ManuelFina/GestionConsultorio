using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GestionConsultorio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar(RegistroUsuarioDto dto)
    {
        var resultado = await _authService.RegistrarAsync(dto);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return Ok(resultado.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var resultado = await _authService.LoginAsync(dto);

        if (!resultado.Exitoso)
            return BadRequest(resultado.Mensaje);

        return Ok(resultado.Data);
    }
}