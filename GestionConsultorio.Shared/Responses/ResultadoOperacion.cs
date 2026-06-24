namespace GestionConsultorio.Shared.Responses;

public class ResultadoOperacion<T>
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ResultadoOperacion<T> Ok(T data)
    {
        return new ResultadoOperacion<T>
        {
            Exitoso = true,
            Data = data
        };
    }

    public static ResultadoOperacion<T> Error(string mensaje)
    {
        return new ResultadoOperacion<T>
        {
            Exitoso = false,
            Mensaje = mensaje
        };
    }
}