using AppMeteoMAUI.Model;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppMeteoMAUI.ViewModel
{
    public partial class HourDetailViewModel : ObservableObject
    {
        public HourDetailViewModel(ForecastDaily fore) 
        {
            forecast = fore;
            PrendiValoriDaVisualizzare();
        }
        ForecastDaily forecast;

        [ObservableProperty]
        double precipitazioni;
        [ObservableProperty]
        int probPrecipitazioni;
        [ObservableProperty]
        int umidita;
        [ObservableProperty]
        double temp;
        [ObservableProperty]
        double tempPercepita;
        [ObservableProperty]
        double? velVento;
        [ObservableProperty]
        string dirVento;
        [ObservableProperty]
        double uvIndex;
        [ObservableProperty]
        double directRadiation;
        [ObservableProperty]
        double visibility;

        private void PrendiValoriDaVisualizzare()
        {
            if (forecast.CurrentForecast1Day != null)
            {
                Temp = forecast.CurrentForecast1Day.Temperature2m;
                Precipitazioni = forecast.CurrentForecast1Day.Precipitation;
                ProbPrecipitazioni = forecast.CurrentForecast1Day.PrecipitationProbability;
                Umidita = forecast.CurrentForecast1Day.Relativehumidity2m;
                TempPercepita = forecast.CurrentForecast1Day.ApparentTemperature;
                VelVento = forecast.CurrentForecast1Day.VelVento;
                DirVento = forecast.CurrentForecast1Day.DirVento;
                UvIndex = forecast.CurrentForecast1Day.UvIndex;
                DirectRadiation = forecast.CurrentForecast1Day.DirectRadiation;
                Visibility = forecast.CurrentForecast1Day.Visibility;
            }
        }
    }
}
