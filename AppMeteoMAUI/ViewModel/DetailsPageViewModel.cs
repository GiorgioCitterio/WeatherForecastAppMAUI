using AppMeteoMAUI.Model;
using AppMeteoMAUI.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppMeteoMAUI.ViewModel
{
    public partial class DetailsPageViewModel : ObservableObject
    {
        public DetailsPageViewModel(ForecastDaily fore)
        {
            currentForecast = new ObservableCollection<ForecastDaily>();
            Forecast = fore;
            StampaDati();
        }
        public ObservableCollection<ForecastDaily> currentForecast { get; set; }

        [ObservableProperty]
        ForecastDaily forecast;
        public static Hourly mioHourly = new();
        [ObservableProperty]
        int? alba;
        [ObservableProperty]
        int? tramonto;

        private async void StampaDati()
        {
            await PrendiSottoinsiemi();
            if (Forecast.Hourly != null)
            {
                for (int i = 0; i < mioHourly.Time.Count; i++)
                {
                    (string, ImageSource) datiImmagine = WMOCodesIntIT(mioHourly.Weathercode[i]);
                    CurrentForecast1Day objCur = new()
                    {
                        Temperature2m = mioHourly.Temperature2m[i],
                        ApparentTemperature = mioHourly.ApparentTemperature[i],
                        DescMeteo = datiImmagine.Item1,
                        ImageUrl = datiImmagine.Item2,
                        Time = UnixTimeStampToDateTime(mioHourly.Time[i]),
                        VelVento = mioHourly.Windspeed10m[i],
                        DirVento = ConvertWindDirectionToString(mioHourly.Winddirection10m[i])
                    };
                    currentForecast.Add(new ForecastDaily() { CurrentForecast1Day = objCur });
                }
                int giorno = (int)Forecast.CurrentForecast.GiornoDellaSettimana;
                Alba = UnixTimeStampToDateTime(Forecast.Daily.Sunrise[giorno]);
                Tramonto = UnixTimeStampToDateTime(Forecast.Daily.Sunset[giorno]);
            }
        }
        #region Metodi Aggiuntivi
        private static int? UnixTimeStampToDateTime(double? unixTimeStamp)
        {
            if (unixTimeStamp != null)
            {
                DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds((double)unixTimeStamp).ToLocalTime();
                return dateTime.Hour;
            }
            return null;
        }
        private static (string, ImageSource) WMOCodesIntIT(int? code)
        {
            return code switch
            {
                0 => ("cielo sereno", ImageSource.FromFile("clear_day.svg")),
                1 => ("limpido", ImageSource.FromFile("partly_cloudy_day.svg")),
                2 => ("annuvolato", ImageSource.FromFile("cloudy.svg")),
                3 => ("coperto", ImageSource.FromFile("extreme_rain.svg")),
                45 => ("nebbia", ImageSource.FromFile("fog.svg")),
                48 => ("brina", ImageSource.FromFile("extreme_fog.svg")),
                51 => ("pioggerella", ImageSource.FromFile("drizzle.svg")),
                53 => ("pioggerella", ImageSource.FromFile("drizzle.svg")),
                55 => ("pioggerella intensa", ImageSource.FromFile("drizzle.svg")),
                56 => ("pioggerella gelata", ImageSource.FromFile("sleet.svg")),
                57 => ("pioggerella gelata", ImageSource.FromFile("extreme_sleet.svg")),
                61 => ("pioggia scarsa", ImageSource.FromFile("drizzle.svg")),
                63 => ("pioggia moderata", ImageSource.FromFile("drizzle.svg")),
                65 => ("pioggia intensa", ImageSource.FromFile("extrene_drizzle.svg")),
                66 => ("pioggia gelata", ImageSource.FromFile("sleet.svg")),
                67 => ("pioggia gelata", ImageSource.FromFile("extreme_sleet.svg")),
                71 => ("nevicata lieve", ImageSource.FromFile("snow.svg")),
                73 => ("nevicata media", ImageSource.FromFile("snow.svg")),
                75 => ("nevicata intensa", ImageSource.FromFile("extreme_snow.svg")),
                77 => ("granelli di neve", ImageSource.FromFile("sleet.svg")),
                80 => ("pioggia debole", ImageSource.FromFile("drizzle.svg")),
                81 => ("pioggia moderata", ImageSource.FromFile("drizzle.svg")),
                82 => ("pioggia violenta", ImageSource.FromFile("extreme_drizzle.svg")),
                85 => ("neve leggera", ImageSource.FromFile("snow.svg")),
                86 => ("neve pesante", ImageSource.FromFile("extreme_snow.svg")),
                95 => ("temporale lieve", ImageSource.FromFile("drizzle.svg")),
                96 => ("temporale grandine", ImageSource.FromFile("sleet.svg")),
                99 => ("temporale grandine", ImageSource.FromFile("extreme_sleet.svg"))
            };
        }
        private async Task PrendiSottoinsiemi()
        {
            List<List<double>> sottoinsiemiTemp = new();
            List<List<int>> sottoinsiemiWeatherCode = new();
            List<List<double>> sottoinsiemiTempApp = new();
            List<List<int>> sottoinsiemiTime = new();
            List<List<double>> sottoinsiemiWind = new ();
            List<List<int>> sottoinsiemiWindDirection = new();
            for (int i = 0; i < Forecast.Hourly.Temperature2m.Count; i += 24)
            {
                sottoinsiemiTemp.Add(Forecast.Hourly.Temperature2m.Skip(i).Take(24).ToList());
                sottoinsiemiWeatherCode.Add(Forecast.Hourly.Weathercode.Skip(i).Take(24).ToList());
                sottoinsiemiTempApp.Add(Forecast.Hourly.ApparentTemperature.Skip(i).Take(24).ToList());
                sottoinsiemiTime.Add(Forecast.Hourly.Time.Skip(i).Take(24).ToList());
                sottoinsiemiWind.Add(Forecast.Hourly.Windspeed10m.Skip(i).Take(24).ToList());
                sottoinsiemiWindDirection.Add(Forecast.Hourly.Winddirection10m.Skip(i).Take(24).ToList());
            }
            switch (Forecast.CurrentForecast.GiornoDellaSettimana)
            {
                case 0:
                    mioHourly.Temperature2m = sottoinsiemiTemp[0];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[0];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[0];
                    mioHourly.Time = sottoinsiemiTime[0];
                    mioHourly.Windspeed10m = sottoinsiemiWind[0];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[0];
                    break;
                case 1:
                    mioHourly.Temperature2m = sottoinsiemiTemp[1];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[1];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[1];
                    mioHourly.Time = sottoinsiemiTime[1];
                    mioHourly.Windspeed10m = sottoinsiemiWind[1];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[1];
                    break;
                case 2:
                    mioHourly.Temperature2m = sottoinsiemiTemp[2];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[2];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[2];
                    mioHourly.Time = sottoinsiemiTime[2];
                    mioHourly.Windspeed10m = sottoinsiemiWind[2];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[2];
                    break;
                case 3:
                    mioHourly.Temperature2m = sottoinsiemiTemp[3];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[3];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[3];
                    mioHourly.Time = sottoinsiemiTime[3];
                    mioHourly.Windspeed10m = sottoinsiemiWind[3];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[3];
                    break;
                case 4:
                    mioHourly.Temperature2m = sottoinsiemiTemp[4];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[4];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[4];
                    mioHourly.Time = sottoinsiemiTime[4];
                    mioHourly.Windspeed10m = sottoinsiemiWind[4];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[4];
                    break;
                case 5:
                    mioHourly.Temperature2m = sottoinsiemiTemp[5];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[5];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[5];
                    mioHourly.Time = sottoinsiemiTime[5];
                    mioHourly.Windspeed10m = sottoinsiemiWind[5];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[5];
                    break;
                case 6:
                    mioHourly.Temperature2m = sottoinsiemiTemp[6];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[6];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[6];
                    mioHourly.Time = sottoinsiemiTime[6];
                    mioHourly.Windspeed10m = sottoinsiemiWind[6];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[6];
                    break;
            }
        }
        private string ConvertWindDirectionToString(int degree)
        {
            if (degree > 337.5) return "N";
            if (degree > 292.5) return "NW";
            if (degree > 247.5) return "W";
            if (degree > 202.5) return "SW";
            if (degree > 157.5) return "S";
            if (degree > 122.5) return "SE";
            if (degree > 67.5) return "E";
            if (degree > 22.5) return "NE";
            return "N";
        }
        #endregion

        #region Pagina Dettagli
        [RelayCommand]
        private async Task GoToDetailsHour(ForecastDaily forecastDaily)
        {
            if (forecastDaily == null)
                return;
            HourDetailViewModel viewModel = new HourDetailViewModel(forecastDaily);
            await App.Current.MainPage.Navigation.PushAsync(new HourDetailPage(viewModel));
        }
        #endregion
    }   
}
