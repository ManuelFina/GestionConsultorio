using GestionConsultorio.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionConsultorio.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Medico> Medicos { get; set; }
    public DbSet<Especialidad> Especialidades { get; set; }
    public DbSet<Consultorio> Consultorios { get; set; }
    public DbSet<Turno> Turnos { get; set; }
    public DbSet<HistorialClinico> HistorialesClinicos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}