namespace GestionConsultorio.Shared.Models;

public class HistorialClinico
{
    public int Id { get; set; }

    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }

    public int TurnoId { get; set; }
    public Turno? Turno { get; set; }

    public string Diagnostico { get; set; } = string.Empty;
    public string Tratamiento { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}