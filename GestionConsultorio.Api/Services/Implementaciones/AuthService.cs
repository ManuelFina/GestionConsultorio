using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Auth;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;
using Microsoft.IdentityModel.Tokens;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class AuthService(IAuthRepository authRepository, IConfiguration configuration) : IAuthService
{
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<ResultadoOperacion<AuthResponseDto>> RegistrarAsync(RegistroUsuarioDto dto)
    {
        var existeEmail = await _authRepository.ExisteEmailAsync(dto.Email);

        if (existeEmail)
            return ResultadoOperacion<AuthResponseDto>.Error("Ya existe un usuario registrado con ese email.");

        if (dto.Rol != "Recepcionista" && dto.Rol != "Medico")
            return ResultadoOperacion<AuthResponseDto>.Error("El rol debe ser Recepcionista o Medico.");

        var usuario = new Usuario
        {
            NombreCompleto = dto.NombreCompleto,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = dto.Rol
        };

        await _authRepository.CrearUsuarioAsync(usuario);

        var response = CrearAuthResponse(usuario);

        return ResultadoOperacion<AuthResponseDto>.Ok(response);
    }

    public async Task<ResultadoOperacion<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var usuario = await _authRepository.ObtenerPorEmailAsync(dto.Email);

        if (usuario is null)
            return ResultadoOperacion<AuthResponseDto>.Error("Email o contraseña incorrectos.");

        var passwordValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);

        if (!passwordValida)
            return ResultadoOperacion<AuthResponseDto>.Error("Email o contraseña incorrectos.");

        var response = CrearAuthResponse(usuario);

        return ResultadoOperacion<AuthResponseDto>.Ok(response);
    }

    private AuthResponseDto CrearAuthResponse(Usuario usuario)
    {
        return new AuthResponseDto
        {
            UsuarioId = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Token = GenerarToken(usuario)
        };
    }

    private string GenerarToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.NombreCompleto),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, usuario.Rol)
        };

        var key = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("La clave JWT no está configurada.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        var expiresInMinutes = Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"]);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiresInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}