namespace GestionConsultorio.Shared.Models;

public class Medico
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public int EspecialidadId { get; set; }
    public Especialidad? Especialidad { get; set; }
}