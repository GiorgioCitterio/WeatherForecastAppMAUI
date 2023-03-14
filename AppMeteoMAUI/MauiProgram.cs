using Microsoft.Extensions.Logging;

namespace AppMeteoMAUI;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("times new roman.ttf", "TimesNewRoman");
                fonts.AddFont("FiraSans-Bold.otf", "FiraSansBold");
                fonts.AddFont("Purple Smile.ttf", "PurpleSmile");
                fonts.AddFont("Requiem.ttf", "Requiem");
                fonts.AddFont("Low Budget.ttf", "LowBudget");
            });

#if DEBUG
		builder.Logging.AddDebug();
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
#endif

        return builder.Build();
	}
}
