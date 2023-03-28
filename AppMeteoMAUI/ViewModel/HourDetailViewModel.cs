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
            }
        }
    }
}
