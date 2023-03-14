using System.Text.Json.Serialization;

namespace AppMeteoMAUI.Model
{
    public class Daily1
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
    }

    public class DailyUnits1
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

        //[JsonPropertyName("daily_units")]
        //public DailyUnits DailyUnits { get; set; }

        [JsonPropertyName("daily")]
        public Daily1 Daily { get; set; }
    }
}
