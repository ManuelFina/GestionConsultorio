namespace GestionConsultorio.Shared.Responses;

public class ApiResponse<T>
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data)
    {
        return new ApiResponse<T>
        {
            Exitoso = true,
            Data = data
        };
    }

    public static ApiResponse<T> Error(string mensaje)
    {
        return new ApiResponse<T>
        {
            Exitoso = false,
            Mensaje = mensaje
        };
    }
}