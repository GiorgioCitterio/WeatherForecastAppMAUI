namespace AppMeteoMAUI.ViewModel
{
    public partial class HourDetailViewModel : ObservableObject
    {
        private ForecastDaily Forecast;
        [ObservableProperty] private double precipitazioni;
        [ObservableProperty] private int probPrecipitazioni;
        [ObservableProperty] private int umidita;
        [ObservableProperty] private double temp;
        [ObservableProperty] private double tempPercepita;
        [ObservableProperty] private double? velVento;
        [ObservableProperty] private string dirVento;
        [ObservableProperty] private double uvIndex;
        [ObservableProperty] private double directRadiation;
        [ObservableProperty] private double visibility;
        public HourDetailViewModel(ForecastDaily forecast)
        {
            this.Forecast = forecast;
            PrendiValoriDaVisualizzare();
        }

        public void PrendiValoriDaVisualizzare()
        {
            if (Forecast.CurrentForecast1Day != null)
            {
                Temp = Forecast.CurrentForecast1Day.Temperature2m;
                Precipitazioni = Forecast.CurrentForecast1Day.Precipitation;
                ProbPrecipitazioni = Forecast.CurrentForecast1Day.PrecipitationProbability;
                Umidita = Forecast.CurrentForecast1Day.Relativehumidity2m;
                TempPercepita = Forecast.CurrentForecast1Day.ApparentTemperature;
                VelVento = Forecast.CurrentForecast1Day.VelVento;
                DirVento = Forecast.CurrentForecast1Day.DirVento;
                UvIndex = Forecast.CurrentForecast1Day.UvIndex;
                DirectRadiation = Forecast.CurrentForecast1Day.DirectRadiation;
                Visibility = Forecast.CurrentForecast1Day.Visibility;
            }
        }
    }
}
