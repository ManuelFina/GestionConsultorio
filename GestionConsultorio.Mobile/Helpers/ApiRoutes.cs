namespace GestionConsultorio.Mobile.Helpers;

public static class ApiRoutes
{
    public const string Pacientes = "api/pacientes";
    public const string Medicos = "api/medicos";
    public const string Especialidades = "api/especialidades";
    public const string Consultorios = "api/consultorios";
    public const string Turnos = "api/turnos";
    public const string HistorialesClinicos = "api/historialesclinicos";
    public const string Auth = "api/auth";
    public const string Recepcionistas = "api/recepcionistas";

    public static string ArchivosHistorialClinico(int historialClinicoId)
    {
        return $"{HistorialesClinicos}/{historialClinicoId}/archivos";
    }

    public static string ArchivoHistorialClinico(int archivoId)
    {
        return $"{HistorialesClinicos}/archivos/{archivoId}";
    }
}