namespace GestionConsultorio.Shared.Models;

public class Paciente
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }

    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;

    public string ObraSocial { get; set; } = string.Empty;
    public string NumeroAfiliado { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }
}