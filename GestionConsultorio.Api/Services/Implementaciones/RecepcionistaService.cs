using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Api.Services.Interfaces;
using GestionConsultorio.Shared.DTOs.Auth;
using GestionConsultorio.Shared.DTOs.Recepcionistas;
using GestionConsultorio.Shared.Models;
using GestionConsultorio.Shared.Responses;

namespace GestionConsultorio.Api.Services.Implementaciones;

public class RecepcionistaService(IAuthRepository authRepository) : IRecepcionistaService
{
    private readonly IAuthRepository _authRepository = authRepository;

    public async Task<ResultadoOperacion<List<RecepcionistaDto>>> ObtenerTodosAsync()
    {
        var recepcionistas = await _authRepository.ObtenerRecepcionistasAsync();

        var dto = recepcionistas
            .Select(MapearDto)
            .ToList();

        return ResultadoOperacion<List<RecepcionistaDto>>.Ok(dto);
    }

    public async Task<ResultadoOperacion<RecepcionistaDto>> ObtenerPorIdAsync(int id)
    {
        var recepcionista = await _authRepository.ObtenerRecepcionistaPorIdAsync(id);

        if (recepcionista is null)
            return ResultadoOperacion<RecepcionistaDto>.Error("Recepcionista no encontrada.");

        return ResultadoOperacion<RecepcionistaDto>.Ok(MapearDto(recepcionista));
    }

    public async Task<ResultadoOperacion<RecepcionistaDto>> CrearAsync(RegistroUsuarioDto dto)
    {
        Normalizar(dto);

        dto.Rol = "Recepcionista";

        var validacion = await ValidarCreacionAsync(dto);

        if (!validacion.Exitoso)
            return ResultadoOperacion<RecepcionistaDto>.Error(validacion.Mensaje);

        var usuario = new Usuario
        {
            NombreCompleto = dto.NombreCompleto,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = "Recepcionista"
        };

        await _authRepository.CrearUsuarioAsync(usuario);

        return ResultadoOperacion<RecepcionistaDto>.Ok(MapearDto(usuario));
    }

    public async Task<ResultadoOperacion<RecepcionistaDto>> ActualizarAsync(int id, ActualizarRecepcionistaDto dto)
    {
        Normalizar(dto);

        if (string.IsNullOrWhiteSpace(dto.NombreCompleto))
            return ResultadoOperacion<RecepcionistaDto>.Error("El nombre completo es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return ResultadoOperacion<RecepcionistaDto>.Error("El email es obligatorio.");

        var recepcionista = await _authRepository.ObtenerRecepcionistaPorIdAsync(id);

        if (recepcionista is null)
            return ResultadoOperacion<RecepcionistaDto>.Error("Recepcionista no encontrada.");

        var usuarioConMismoEmail = await _authRepository.ObtenerPorEmailAsync(dto.Email);

        if (usuarioConMismoEmail is not null && usuarioConMismoEmail.Id != id)
            return ResultadoOperacion<RecepcionistaDto>.Error("Ya existe un usuario registrado con ese email.");

        recepcionista.NombreCompleto = dto.NombreCompleto;
        recepcionista.Email = dto.Email;
        recepcionista.Rol = "Recepcionista";

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            if (dto.Password.Length < 6)
                return ResultadoOperacion<RecepcionistaDto>.Error("La contraseña debe tener al menos 6 caracteres.");

            recepcionista.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        await _authRepository.ActualizarUsuarioAsync(recepcionista);

        return ResultadoOperacion<RecepcionistaDto>.Ok(MapearDto(recepcionista));
    }

    public async Task<ResultadoOperacion<bool>> EliminarAsync(int id)
    {
        var recepcionista = await _authRepository.ObtenerRecepcionistaPorIdAsync(id);

        if (recepcionista is null)
            return ResultadoOperacion<bool>.Error("Recepcionista no encontrada.");

        await _authRepository.EliminarUsuarioAsync(recepcionista);

        return ResultadoOperacion<bool>.Ok(true);
    }

    private async Task<ResultadoOperacion<bool>> ValidarCreacionAsync(RegistroUsuarioDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NombreCompleto))
            return ResultadoOperacion<bool>.Error("El nombre completo es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return ResultadoOperacion<bool>.Error("El email es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            return ResultadoOperacion<bool>.Error("La contraseña es obligatoria.");

        if (dto.Password.Length < 6)
            return ResultadoOperacion<bool>.Error("La contraseña debe tener al menos 6 caracteres.");

        var existeEmail = await _authRepository.ExisteEmailAsync(dto.Email);

        if (existeEmail)
            return ResultadoOperacion<bool>.Error("Ya existe un usuario registrado con ese email.");

        return ResultadoOperacion<bool>.Ok(true);
    }

    private static RecepcionistaDto MapearDto(Usuario usuario)
    {
        return new RecepcionistaDto
        {
            Id = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Email = usuario.Email,
            Rol = usuario.Rol
        };
    }

    private static void Normalizar(RegistroUsuarioDto dto)
    {
        dto.NombreCompleto = dto.NombreCompleto?.Trim() ?? string.Empty;
        dto.Email = dto.Email?.Trim() ?? string.Empty;
        dto.Password = dto.Password?.Trim() ?? string.Empty;
        dto.Rol = "Recepcionista";
    }

    private static void Normalizar(ActualizarRecepcionistaDto dto)
    {
        dto.NombreCompleto = dto.NombreCompleto?.Trim() ?? string.Empty;
        dto.Email = dto.Email?.Trim() ?? string.Empty;
        dto.Password = dto.Password?.Trim();
    }
}