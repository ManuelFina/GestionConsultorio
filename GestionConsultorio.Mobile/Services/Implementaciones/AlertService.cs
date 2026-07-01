using GestionConsultorio.Mobile.Services.Interfaces;
using Microsoft.JSInterop;

namespace GestionConsultorio.Mobile.Services.Implementaciones;

public class AlertService : IAlertService
{
    private readonly IJSRuntime _jsRuntime;

    public AlertService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SuccessAsync(string title, string text)
    {
        await _jsRuntime.InvokeVoidAsync("appAlerts.success", title, text);
    }

    public async Task ErrorAsync(string title, string text)
    {
        await _jsRuntime.InvokeVoidAsync("appAlerts.error", title, text);
    }

    public async Task WarningAsync(string title, string text)
    {
        await _jsRuntime.InvokeVoidAsync("appAlerts.warning", title, text);
    }

    public async Task InfoAsync(string title, string text)
    {
        await _jsRuntime.InvokeVoidAsync("appAlerts.info", title, text);
    }

    public async Task<bool> ConfirmAsync(string title, string text, string confirmButtonText = "Confirmar")
    {
        return await _jsRuntime.InvokeAsync<bool>("appAlerts.confirm", title, text, confirmButtonText);
    }
}