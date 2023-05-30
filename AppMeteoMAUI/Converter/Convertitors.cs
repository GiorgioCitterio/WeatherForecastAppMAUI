using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMeteoMAUI.Converter
{
    public class Convertitors
    {
        public static DateTime? UnixTimeStampToDateTime(double? unixTimeStamp)
        {
            if (unixTimeStamp != null)
            {
                DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds((double)unixTimeStamp).ToLocalTime();
                return dateTime;
            }
            return null;
        }
        public static (string, ImageSource) WMOCodesIntIT(int? code)
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
        public static string ConvertWindDirectionToString(int degree)
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
    }
}
