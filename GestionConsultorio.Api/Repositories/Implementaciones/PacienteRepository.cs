using GestionConsultorio.Api.Data;
using GestionConsultorio.Api.Repositories.Interfaces;
using GestionConsultorio.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionConsultorio.Api.Repositories.Implementaciones;

public class PacienteRepository(AppDbContext context) : Repository<Paciente>(context), IPacienteRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Paciente?> ObtenerPorDniAsync(string dni)
    {
        return await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Dni == dni);
    }

    public async Task<bool> ExisteDniAsync(string dni)
    {
        return await _context.Pacientes
            .AnyAsync(p => p.Dni == dni);
    }
}