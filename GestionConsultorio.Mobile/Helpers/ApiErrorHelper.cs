using System.Net;

namespace GestionConsultorio.Mobile.Helpers;

public static class ApiErrorHelper
{
    public static async Task<string> ObtenerMensajeErrorAsync(HttpResponseMessage response)
    {
        var mensaje = await response.Content.ReadAsStringAsync();

        if (!string.IsNullOrWhiteSpace(mensaje))
            return mensaje;

        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => "La solicitud no es válida.",
            HttpStatusCode.Unauthorized => "No estás autorizado para realizar esta acción.",
            HttpStatusCode.Forbidden => "No tenés permisos para realizar esta acción.",
            HttpStatusCode.NotFound => "El recurso solicitado no fue encontrado.",
            HttpStatusCode.InternalServerError => "Error interno del servidor.",
            _ => "Ocurrió un error al procesar la solicitud."
        };
    }
}