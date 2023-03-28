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
        public static Hourly mioHourly = new();
        [ObservableProperty]
        int? alba;
        [ObservableProperty]
        int? tramonto;
        [ObservableProperty]
        string descrizioneGiornata;

        private void StampaDati()
        {
            PrendiSottoinsiemi();
            if (Forecast.Hourly != null)
            {
                int giorno = (int)Forecast.CurrentForecast.GiornoDellaSettimana;
                double? tempMedia = (Forecast.Daily.Temperature2mMax[giorno] + Forecast.Daily.Temperature2mMin[giorno]) / 2;
                Alba = UnixTimeStampToDateTime(Forecast.Daily.Sunrise[giorno]);
                Tramonto = UnixTimeStampToDateTime(Forecast.Daily.Sunset[giorno]);
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
                        DirVento = ConvertWindDirectionToString(mioHourly.Winddirection10m[i]),
                        OraDelGiorno = i,
                        Precipitation = mioHourly.Precipitation[i],
                        PrecipitationProbability = mioHourly.PrecipitationProbability[i],
                        Relativehumidity2m = mioHourly.Relativehumidity2m[i]
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
                DescrizioneGiornata = DayDescription(tempMedia, windMedio);
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
        private void PrendiSottoinsiemi()
        {
            List<List<double>> sottoinsiemiTemp = new();
            List<List<int>> sottoinsiemiWeatherCode = new();
            List<List<double>> sottoinsiemiTempApp = new();
            List<List<int>> sottoinsiemiTime = new();
            List<List<double>> sottoinsiemiWind = new ();
            List<List<int>> sottoinsiemiWindDirection = new();
            List<List<double>> sottoinsiemiPrecipitation = new();
            List<List<int>> sottoinsiemiPrecipitationProb = new();
            List<List<int>> sottoinsiemiUmid = new();
            for (int i = 0; i < Forecast.Hourly.Temperature2m.Count; i += 24)
            {
                sottoinsiemiTemp.Add(Forecast.Hourly.Temperature2m.Skip(i).Take(24).ToList());
                sottoinsiemiWeatherCode.Add(Forecast.Hourly.Weathercode.Skip(i).Take(24).ToList());
                sottoinsiemiTempApp.Add(Forecast.Hourly.ApparentTemperature.Skip(i).Take(24).ToList());
                sottoinsiemiTime.Add(Forecast.Hourly.Time.Skip(i).Take(24).ToList());
                sottoinsiemiWind.Add(Forecast.Hourly.Windspeed10m.Skip(i).Take(24).ToList());
                sottoinsiemiWindDirection.Add(Forecast.Hourly.Winddirection10m.Skip(i).Take(24).ToList());
                sottoinsiemiPrecipitation.Add(Forecast.Hourly.Precipitation.Skip(i).Take(24).ToList());
                sottoinsiemiPrecipitationProb.Add(Forecast.Hourly.PrecipitationProbability.Skip(i).Take(24).ToList());
                sottoinsiemiUmid.Add(Forecast.Hourly.Relativehumidity2m.Skip(i).Take(24).ToList());
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
                    mioHourly.Precipitation = sottoinsiemiPrecipitation[0];
                    mioHourly.PrecipitationProbability = sottoinsiemiPrecipitationProb[0];
                    mioHourly.Relativehumidity2m = sottoinsiemiUmid[0];
                    break;
                case 1:
                    mioHourly.Temperature2m = sottoinsiemiTemp[1];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[1];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[1];
                    mioHourly.Time = sottoinsiemiTime[1];
                    mioHourly.Windspeed10m = sottoinsiemiWind[1];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[1];
                    mioHourly.Precipitation = sottoinsiemiPrecipitation[1];
                    mioHourly.PrecipitationProbability = sottoinsiemiPrecipitationProb[1];
                    mioHourly.Relativehumidity2m = sottoinsiemiUmid[1];
                    break;
                case 2:
                    mioHourly.Temperature2m = sottoinsiemiTemp[2];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[2];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[2];
                    mioHourly.Time = sottoinsiemiTime[2];
                    mioHourly.Windspeed10m = sottoinsiemiWind[2];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[2];
                    mioHourly.Precipitation = sottoinsiemiPrecipitation[2];
                    mioHourly.PrecipitationProbability = sottoinsiemiPrecipitationProb[2];
                    mioHourly.Relativehumidity2m = sottoinsiemiUmid[2];
                    break;
                case 3:
                    mioHourly.Temperature2m = sottoinsiemiTemp[3];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[3];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[3];
                    mioHourly.Time = sottoinsiemiTime[3];
                    mioHourly.Windspeed10m = sottoinsiemiWind[3];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[3];
                    mioHourly.Precipitation = sottoinsiemiPrecipitation[3];
                    mioHourly.PrecipitationProbability = sottoinsiemiPrecipitationProb[3];
                    mioHourly.Relativehumidity2m = sottoinsiemiUmid[3];
                    break;
                case 4:
                    mioHourly.Temperature2m = sottoinsiemiTemp[4];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[4];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[4];
                    mioHourly.Time = sottoinsiemiTime[4];
                    mioHourly.Windspeed10m = sottoinsiemiWind[4];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[4];
                    mioHourly.Precipitation = sottoinsiemiPrecipitation[4];
                    mioHourly.PrecipitationProbability = sottoinsiemiPrecipitationProb[4];
                    mioHourly.Relativehumidity2m = sottoinsiemiUmid[4];
                    break;
                case 5:
                    mioHourly.Temperature2m = sottoinsiemiTemp[5];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[5];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[5];
                    mioHourly.Time = sottoinsiemiTime[5];
                    mioHourly.Windspeed10m = sottoinsiemiWind[5];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[5];
                    mioHourly.Precipitation = sottoinsiemiPrecipitation[5];
                    mioHourly.PrecipitationProbability = sottoinsiemiPrecipitationProb[5];
                    mioHourly.Relativehumidity2m = sottoinsiemiUmid[5];
                    break;
                case 6:
                    mioHourly.Temperature2m = sottoinsiemiTemp[6];
                    mioHourly.Weathercode = sottoinsiemiWeatherCode[6];
                    mioHourly.ApparentTemperature = sottoinsiemiTempApp[6];
                    mioHourly.Time = sottoinsiemiTime[6];
                    mioHourly.Windspeed10m = sottoinsiemiWind[6];
                    mioHourly.Winddirection10m = sottoinsiemiWindDirection[6];
                    mioHourly.Precipitation = sottoinsiemiPrecipitation[6];
                    mioHourly.PrecipitationProbability = sottoinsiemiPrecipitationProb[6];
                    mioHourly.Relativehumidity2m = sottoinsiemiUmid[6];
                    break;
            }
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
