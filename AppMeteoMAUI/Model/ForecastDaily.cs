using System.Text.Json.Serialization;

namespace AppMeteoMAUI.Model
{
    public class CurrentWeather
    {
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("windspeed")]
        public double Windspeed { get; set; }

        [JsonPropertyName("weathercode")]
        public int Weathercode { get; set; }

        [JsonPropertyName("time")]
        public int Time { get; set; }
    }

    public class Daily
    {
        [JsonPropertyName("time")]
        public List<int?> Time { get; set; }

        [JsonPropertyName("weathercode")]
        public List<int?> Weathercode { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public List<double?> Temperature2mMax { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double?> Temperature2mMin { get; set; }

        [JsonPropertyName("sunrise")]
        public List<int?> Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public List<int?> Sunset { get; set; }

        [JsonPropertyName("apparent_temperature_max")]
        public List<double> ApparentTemperatureMax { get; set; }

        [JsonPropertyName("apparent_temperature_min")]
        public List<double> ApparentTemperatureMin { get; set; }
    }

    public class DailyUnits
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("weathercode")]
        public string Weathercode { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public string Temperature2mMax { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public string Temperature2mMin { get; set; }

        [JsonPropertyName("sunrise")]
        public string Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public string Sunset { get; set; }

        [JsonPropertyName("apparent_temperature_max")]
        public string ApparentTemperatureMax { get; set; }

        [JsonPropertyName("apparent_temperature_min")]
        public string ApparentTemperatureMin { get; set; }
    }

    public class Hourly
    {
        [JsonPropertyName("time")]
        public List<int> Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public List<double> Temperature2m { get; set; }

        [JsonPropertyName("weathercode")]
        public List<int> Weathercode { get; set; }

        [JsonPropertyName("windspeed_10m")]
        public List<double> Windspeed10m { get; set; }

        [JsonPropertyName("winddirection_10m")]
        public List<int> Winddirection10m { get; set; }

        [JsonPropertyName("apparent_temperature")]
        public List<double> ApparentTemperature { get; set; }

        [JsonPropertyName("precipitation_probability")]
        public List<int> PrecipitationProbability { get; set; }

        [JsonPropertyName("precipitation")]
        public List<double> Precipitation { get; set; }

        [JsonPropertyName("showers")]
        public List<double> Showers { get; set; }
    }

    public class HourlyUnits
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public string Temperature2m { get; set; }

        [JsonPropertyName("weathercode")]
        public string Weathercode { get; set; }

        [JsonPropertyName("windspeed_10m")]
        public string Windspeed10m { get; set; }

        [JsonPropertyName("winddirection_10m")]
        public string Winddirection10m { get; set; }

        [JsonPropertyName("apparent_temperature")]
        public string ApparentTemperature { get; set; }

        [JsonPropertyName("precipitation_probability")]
        public string PrecipitationProbability { get; set; }

        [JsonPropertyName("precipitation")]
        public string Precipitation { get; set; }

        [JsonPropertyName("showers")]
        public string Showers { get; set; }
    }

    public class ForecastDaily
    {
        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("generationtime_ms")]
        public double? GenerationtimeMs { get; set; }

        [JsonPropertyName("utc_offset_seconds")]
        public int? UtcOffsetSeconds { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("timezone_abbreviation")]
        public string TimezoneAbbreviation { get; set; }

        [JsonPropertyName("elevation")]
        public double? Elevation { get; set; }

        [JsonPropertyName("current_weather")]
        public CurrentWeather CurrentWeather { get; set; }

        [JsonPropertyName("hourly_units")]
        public HourlyUnits HourlyUnits { get; set; }

        [JsonPropertyName("hourly")]
        public Hourly Hourly { get; set; }

        [JsonPropertyName("daily_units")]
        public DailyUnits DailyUnits { get; set; }

        [JsonPropertyName("daily")]
        public Daily Daily { get; set; }

        [JsonPropertyName("current_forecast")]
        public CurrentForecast CurrentForecast { get; set; }

        [JsonPropertyName("current_forecast1Day")]
        public CurrentForecast1Day CurrentForecast1Day { get; set; }
    }

    public class CurrentForecast
    {
        [JsonPropertyName("temperature_2m_max")]
        public double? Temperature2mMax { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public double? Temperature2mMin { get; set; }
        [JsonPropertyName("image_url")]
        public ImageSource ImageUrl { get; set; }
        [JsonPropertyName("desc_meteo")]
        public string DescMeteo { get; set; }
        [JsonPropertyName("data")]
        public DateTime? Data { get; set; }

        [JsonPropertyName("giorno_della_settimana")]
        public int? GiornoDellaSettimana { get; set; }
    }

    public class CurrentForecast1Day
    {
        [JsonPropertyName("time")]
        public int? Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double Temperature2m { get; set; }
        [JsonPropertyName("apparent_temperature")]
        public double ApparentTemperature { get; set; }
        [JsonPropertyName("image_url")]
        public ImageSource ImageUrl { get; set; }
        [JsonPropertyName("desc_meteo")]
        public string DescMeteo { get; set; }
        [JsonPropertyName("vel_vento")]
        public double? VelVento { get; set; }
        [JsonPropertyName("dir_vento")]
        public string? DirVento { get; set; }
    }
}
