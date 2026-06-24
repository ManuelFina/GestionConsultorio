using GestionConsultorio.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionConsultorio.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Medico> Medicos { get; set; }
    public DbSet<Especialidad> Especialidades { get; set; }
    public DbSet<Consultorio> Consultorios { get; set; }
    public DbSet<Turno> Turnos { get; set; }
    public DbSet<HistorialClinico> HistorialesClinicos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HistorialClinico>()
            .HasOne(h => h.Paciente)
            .WithMany()
            .HasForeignKey(h => h.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HistorialClinico>()
            .HasOne(h => h.Turno)
            .WithMany()
            .HasForeignKey(h => h.TurnoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}