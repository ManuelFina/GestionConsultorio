using System.Diagnostics;
using System.Net.Http.Headers;
using GestionConsultorio.Mobile.Services.Interfaces;

namespace GestionConsultorio.Mobile.Helpers;

public partial class AuthHeaderHandler : DelegatingHandler
{
    private readonly ISesionService _sesionService;

    public AuthHeaderHandler(ISesionService sesionService)
    {
        _sesionService = sesionService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _sesionService.ObtenerTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}