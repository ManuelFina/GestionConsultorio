using GestionConsultorio.Shared.Enums;

namespace GestionConsultorio.Shared.Models;

public class Turno
{
    public int Id { get; set; }

    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }

    public int MedicoId { get; set; }
    public Medico? Medico { get; set; }

    public int ConsultorioId { get; set; }
    public Consultorio? Consultorio { get; set; }

    public DateOnly Fecha { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }

    public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;

    public string MotivoConsulta { get; set; } = string.Empty;
}