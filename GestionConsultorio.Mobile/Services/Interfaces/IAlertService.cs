namespace GestionConsultorio.Mobile.Services.Interfaces;

public interface IAlertService
{
    Task SuccessAsync(string title, string text);
    Task ErrorAsync(string title, string text);
    Task WarningAsync(string title, string text);
    Task InfoAsync(string title, string text);
    Task<bool> ConfirmAsync(string title, string text, string confirmButtonText = "Confirmar");
}