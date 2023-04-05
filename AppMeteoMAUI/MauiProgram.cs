using Microsoft.Extensions.Logging;
using AppMeteoMAUI.Service;
using Syncfusion.Maui.Core.Hosting;

namespace AppMeteoMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureSyncfusionCore()
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
#endif
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        string dbPath = FileAccessHelper.GetFileLocalPath("Preferiti.db3");
        builder.Services.AddSingleton<PreferitiRepository>(s => ActivatorUtilities.CreateInstance<PreferitiRepository>(s, dbPath));
        return builder.Build();
	}
}
