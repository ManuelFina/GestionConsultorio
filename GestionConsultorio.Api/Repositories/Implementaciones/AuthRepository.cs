using GestionConsultorio.Api.Data;
using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionConsultorio.Api.Repositories.Implementaciones;

public class AuthRepository(AppDbContext context) : IAuthRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Usuario?> ObtenerPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExisteEmailAsync(string email)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Email == email);
    }

    public async Task CrearUsuarioAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }
}