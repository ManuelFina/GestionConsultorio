using GestionConsultorio.Mobile.Services.Implementaciones;
using GestionConsultorio.Mobile.Services.Interfaces;
using GestionConsultorio.Mobile.Settings;
using Microsoft.Extensions.Logging;

namespace GestionConsultorio.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(ApiSettings.BaseUrl)
        });

        builder.Services.AddScoped<IPacienteService, PacienteService>();
        builder.Services.AddScoped<IPacienteService, PacienteService>();
        builder.Services.AddScoped<IEspecialidadService, EspecialidadService>();
        builder.Services.AddScoped<IConsultorioService, ConsultorioService>();
        builder.Services.AddScoped<IMedicoService, MedicoService>();
        builder.Services.AddScoped<ITurnoService, TurnoService>();
        builder.Services.AddScoped<IHistorialClinicoService, HistorialClinicoService>();

        builder.Services.AddScoped<ISesionService, SesionService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddScoped<IAlertService, AlertService>();

        return builder.Build();
	}
}
