using AppMeteoMAUI.Model;
using AppMeteoMAUI.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Numerics;

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
        public static double? windMedio;

        [ObservableProperty]
        ForecastDaily forecast;
        [ObservableProperty]
        int? alba;
        [ObservableProperty]
        int? tramonto;
        [ObservableProperty]
        string descrizioneGiornata;

        private void StampaDati()
        {
            if (Forecast.Hourly != null)
            {
                var fd = Forecast.Hourly;
                int giorno = (int)Forecast.CurrentForecast.GiornoDellaSettimana;
                //double? tempMedia = (Forecast.Daily.Temperature2mMax[giorno] + Forecast.Daily.Temperature2mMin[giorno]) / 2;
                Alba = UnixTimeStampToDateTime(Forecast.Daily.Sunrise[giorno]);
                Tramonto = UnixTimeStampToDateTime(Forecast.Daily.Sunset[giorno]);
                for (int i = 0; i < 24; i++)
                {
                    int index = giorno * 24 + i;
                    (string, ImageSource) datiImmagine = WMOCodesIntIT(fd.Weathercode[index]);
                    CurrentForecast1Day objCur = new()
                    {
                        Temperature2m = fd.Temperature2m[index],
                        ApparentTemperature = fd.ApparentTemperature[index],
                        DescMeteo = datiImmagine.Item1,
                        ImageUrl = datiImmagine.Item2,
                        Time = UnixTimeStampToDateTime(fd.Time[index]),
                        VelVento = fd.Windspeed10m[index],
                        DirVento = ConvertWindDirectionToString(fd.Winddirection10m[index]),
                        OraDelGiorno = i,
                        Precipitation = fd.Precipitation[index],
                        PrecipitationProbability = fd.PrecipitationProbability[index],
                        Relativehumidity2m = fd.Relativehumidity2m[index]
                    };
                    if (objCur.DescMeteo == "cielo sereno" && (objCur.Time > Tramonto || objCur.Time == 0 ||objCur.Time < Alba))
                    {
                        objCur.ImageUrl = ImageSource.FromFile("clear_night.svg");
                    }
                    else if (objCur.DescMeteo == "limpido" && (objCur.Time > Tramonto || objCur.Time == 0 || objCur.Time < Alba))
                    {
                        objCur.ImageUrl = ImageSource.FromFile("extreme_night.svg");
                    }
                    currentForecast.Add(new ForecastDaily() { CurrentForecast1Day = objCur });
                    windMedio += objCur.VelVento;
                }
                windMedio /= 24;
                //DescrizioneGiornata = DayDescription(tempMedia, windMedio);
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
        private string ConvertWindDirectionToString(int degree)
        {
            if (degree > 337.5) return "N";
            if (degree > 292.5) return "NO";
            if (degree > 247.5) return "O";
            if (degree > 202.5) return "SO";
            if (degree > 157.5) return "S";
            if (degree > 122.5) return "SE";
            if (degree > 67.5) return "E";
            if (degree > 22.5) return "NE";
            return "N";
        }
        private string DayDescription(double? degree, double? windspeed)
        {
            if(windspeed > 0)
            {
                if (windspeed > 5 && windspeed <= 15)
                    if (degree < -30)
                        return "Giornata nordica, brezza leggera";
                    else if (degree > -20 && degree <= -10)
                        return "Giornata gelida, brezza leggera";
                    else if (degree > -10 && degree <= 0)
                        return "Giornata invernale, brezza leggera";
                    else if (degree > 0 && degree <= 10)
                        return "Giornata fredda, brezza leggera";
                    else if (degree > 10 && degree <= 20)
                        return "Giornata temperata, brezza leggera";
                    else if (degree > 20 && degree <= 30)
                        return "Giornata afosa, brezza leggera";
                    else if (degree > 30)
                        return "Giornata torrida, brezza leggera";
                    else
                        return null;
                else if (windspeed > 15 && windspeed <= 30)
                    if (degree < -30)
                        return "Giornata nordica, vento moderato";
                    else if (degree > -20 && degree <= -10)
                        return "Giornata gelida, vento moderato";
                    else if (degree > -10 && degree <= 0)
                        return "Giornata invernale, vento moderato";
                    else if (degree > 0 && degree <= 10)
                        return "Giornata fredda, vento moderato";
                    else if (degree > 10 && degree <= 20)
                        return "Giornata temperata, vento moderato";
                    else if (degree > 20 && degree <= 30)
                        return "Giornata afosa, vento moderato";
                    else if (degree > 30)
                        return "Giornata torrida, vento moderato";
                    else
                        return null;
                else if (windspeed > 30 && windspeed <= 50)
                    if (degree < -30)
                        return "Giornata nordica, vento fresco";
                    else if (degree > -20 && degree <= -10)
                        return "Giornata gelida, vento fresco";
                    else if (degree > -10 && degree <= 0)
                        return "Giornata invernale, vento fresco";
                    else if (degree > 0 && degree <= 10)
                        return "Giornata fredda, vento fresco";
                    else if (degree > 10 && degree <= 20)
                        return "Giornata temperata, vento fresco";
                    else if (degree > 20 && degree <= 30)
                        return "Giornata afosa, vento fresco";
                    else if (degree > 30)
                        return "Giornata torrida, vento fresco";
                    else
                        return null;
                else if (windspeed > 50 && windspeed <= 70)
                    if (degree < -30)
                        return "Giornata nordica, vento forte";
                    else if (degree > -20 && degree <= -10)
                        return "Giornata gelida, vento forte";
                    else if (degree > -10 && degree <= 0)
                        return "Giornata invernale, vento forte";
                    else if (degree > 0 && degree <= 10)
                        return "Giornata fredda, vento forte";
                    else if (degree > 10 && degree <= 20)
                        return "Giornata temperata, vento forte";
                    else if (degree > 20 && degree <= 30)
                        return "Giornata afosa, vento forte";
                    else if (degree > 30)
                        return "Giornata torrida, vento forte";
                    else
                        return null;
                else if (windspeed > 70 && windspeed <= 90)
                    if (degree < -30)
                        return "Giornata nordica, burrasca";
                    else if (degree > -20 && degree <= -10)
                        return "Giornata gelida, burrasca";
                    else if (degree > -10 && degree <= 0)
                        return "Giornata invernale, burrasca";
                    else if (degree > 0 && degree <= 10)
                        return "Giornata fredda, tempesta";
                    else if (degree > 10 && degree <= 20)
                        return "Giornata temperata, burrasca";
                    else if (degree > 20 && degree <= 30)
                        return "Giornata afosa, burrasca";
                    else if (degree > 30)
                        return "Giornata torrida, burrasca";
                    else
                        return null;
                else if (windspeed > 90 && windspeed <= 110)
                    if (degree < -30)
                        return "Giornata nordica, tempesta";
                    else if (degree > -20 && degree <= -10)
                        return "Giornata gelida, tempesta";
                    else if (degree > -10 && degree <= 0)
                        return "Giornata invernale, tempesta";
                    else if (degree > 0 && degree <= 10)
                        return "Giornata fredda, tempesta";
                    else if (degree > 10 && degree <= 20)
                        return "Giornata temperata, tempesta";
                    else if (degree > 20 && degree <= 30)
                        return "Giornata afosa, tempesta";
                    else if (degree > 30)
                        return "Giornata torrida, tempesta";
                    else
                        return null;
                else if (windspeed > 110)
                    if (degree < -30)
                        return "Giornata nordica, uragano";
                    else if (degree > -20 && degree <= -10)
                        return "Giornata gelida, uragano";
                    else if (degree > -10 && degree <= 0)
                        return "Giornata invernale, uragano";
                    else if (degree > 0 && degree <= 10)
                        return "Giornata fredda, uragano";
                    else if (degree > 10 && degree <= 20)
                        return "Giornata temperata, uragano";
                    else if (degree > 20 && degree <= 30)
                        return "Giornata afosa, uragano";
                    else if (degree > 30)
                        return "Giornata torrida, uragano";
                    else
                        return null;
                return null;
            }
            else
            {
                if (degree < -30)
                    return "Giornata nordica";
                else if (degree > -20 && degree <= -10)
                    return "Giornata gelida";
                else if (degree > -10 && degree <= 0)
                    return "Giornata invernale";
                else if (degree > 0 && degree <= 10)
                    return "Giornata fredda";
                else if (degree > 10 && degree <= 20)
                    return "Giornata temperata";
                else if (degree > 20 && degree <= 30)
                    return "Giornata afosa";
                else if (degree > 30)
                    return "Giornata torrida";
                else
                    return null;
            }
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
