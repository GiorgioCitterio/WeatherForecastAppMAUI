using AppMeteoMAUI.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AppMeteoMAUI.ViewModel
{
    [QueryProperty(nameof(ForecastDaily), nameof(ForecastDaily))]
    public partial class HourDetailsViewModel : ObservableObject
    {
        //[ObservableProperty]
        //static ForecastDaily forecastDaily;
        //[ObservableProperty]
        //static double temperatura;
        //[ObservableProperty]
        //static string city;
        //public static ObservableCollection<CurrentForecast1Day> currentForecast { get; set; }
        //public HourDetailsViewModel()
        //{
        //    currentForecast = new ObservableCollection<CurrentForecast1Day>();
        //}
        //public static void StampaDati()
        //{
        //    if (forecastDaily.Hourly != null)
        //    {
        //        var fd = forecastDaily.Hourly;
                
        //        for (int i = 0; i < fd.Time.Count; i++)
        //        {
        //            (string, ImageSource) datiImmagine = WMOCodesIntIT(fd.Weathercode[i]);
        //            currentForecast.Add(new CurrentForecast1Day()
        //            {
        //                Temperature2m = fd.Temperature2m[i],
        //                ApparentTemperature = fd.ApparentTemperature[i],
        //                DescMeteo = datiImmagine.Item1,
        //                ImageUrl = datiImmagine.Item2,
        //                Time = UnixTimeStampToDateTime(fd.Time[i]),

        //            });
        //        }
        //        Temperatura = forecastDaily.CurrentWeather.Temperature;
        //    }
        //}
        //private static int? UnixTimeStampToDateTime(double? unixTimeStamp)
        //{
        //    if (unixTimeStamp != null)
        //    {
        //        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //        dateTime = dateTime.AddSeconds((double)unixTimeStamp).ToLocalTime();
        //        return dateTime.Hour;
        //    }
        //    return null;
        //}
        //static (string, ImageSource) WMOCodesIntIT(int? code)
        //{
        //    return code switch
        //    {
        //        0 => ("cielo sereno", ImageSource.FromFile("clear_day.svg")),
        //        1 => ("limpido", ImageSource.FromFile("partly_cloudy_day.svg")),
        //        2 => ("annuvolato", ImageSource.FromFile("cloudy.svg")),
        //        3 => ("coperto", ImageSource.FromFile("extreme_rain.svg")),
        //        45 => ("nebbia", ImageSource.FromFile("fog.svg")),
        //        48 => ("brina", ImageSource.FromFile("extreme_fog.svg")),
        //        51 => ("pioggerella", ImageSource.FromFile("drizzle.svg")),
        //        53 => ("pioggerella", ImageSource.FromFile("drizzle.svg")),
        //        55 => ("pioggerella intensa", ImageSource.FromFile("drizzle.svg")),
        //        56 => ("pioggerella gelata", ImageSource.FromFile("sleet.svg")),
        //        57 => ("pioggerella gelata", ImageSource.FromFile("extreme_sleet.svg")),
        //        61 => ("pioggia scarsa", ImageSource.FromFile("drizzle.svg")),
        //        63 => ("pioggia moderata", ImageSource.FromFile("drizzle.svg")),
        //        65 => ("pioggia intensa", ImageSource.FromFile("extrene_drizzle.svg")),
        //        66 => ("pioggia gelata", ImageSource.FromFile("sleet.svg")),
        //        67 => ("pioggia gelata", ImageSource.FromFile("extreme_sleet.svg")),
        //        71 => ("nevicata lieve", ImageSource.FromFile("snow.svg")),
        //        73 => ("nevicata media", ImageSource.FromFile("snow.svg")),
        //        75 => ("nevicata intensa", ImageSource.FromFile("extreme_snow.svg")),
        //        77 => ("granelli di neve", ImageSource.FromFile("sleet.svg")),
        //        80 => ("pioggia debole", ImageSource.FromFile("drizzle.svg")),
        //        81 => ("pioggia moderata", ImageSource.FromFile("drizzle.svg")),
        //        82 => ("pioggia violenta", ImageSource.FromFile("extreme_drizzle.svg")),
        //        85 => ("neve leggera", ImageSource.FromFile("snow.svg")),
        //        86 => ("neve pesante", ImageSource.FromFile("extreme_snow.svg")),
        //        95 => ("temporale lieve", ImageSource.FromFile("drizzle.svg")),
        //        96 => ("temporale grandine", ImageSource.FromFile("sleet.svg")),
        //        99 => ("temporale grandine", ImageSource.FromFile("extreme_sleet.svg"))
        //    };
        //}
    }
}
