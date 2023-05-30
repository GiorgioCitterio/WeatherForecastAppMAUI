namespace AppMeteoMAUI.ViewModel
{
    public partial class HourDetailViewModel : ObservableObject
    {
        public HourDetailViewModel(ForecastDaily fore) 
        {
            forecast = fore;
        }

        private static ForecastDaily forecast;
        [ObservableProperty] private static double precipitazioni;
        [ObservableProperty] private static int probPrecipitazioni;
        [ObservableProperty] private static int umidita;
        [ObservableProperty] private static double temp;
        [ObservableProperty] private static double tempPercepita;
        [ObservableProperty] private static double? velVento;
        [ObservableProperty] private static string dirVento;
        [ObservableProperty] private static double uvIndex;
        [ObservableProperty] private static double directRadiation;
        [ObservableProperty] private static double visibility;

        public static void PrendiValoriDaVisualizzare()
        {
            if (forecast.CurrentForecast1Day != null)
            {
                temp = forecast.CurrentForecast1Day.Temperature2m;
                precipitazioni = forecast.CurrentForecast1Day.Precipitation;
                probPrecipitazioni = forecast.CurrentForecast1Day.PrecipitationProbability;
                umidita = forecast.CurrentForecast1Day.Relativehumidity2m;
                tempPercepita = forecast.CurrentForecast1Day.ApparentTemperature;
                velVento = forecast.CurrentForecast1Day.VelVento;
                dirVento = forecast.CurrentForecast1Day.DirVento;
                uvIndex = forecast.CurrentForecast1Day.UvIndex;
                directRadiation = forecast.CurrentForecast1Day.DirectRadiation;
                visibility = forecast.CurrentForecast1Day.Visibility;
            }
        }
    }
}
