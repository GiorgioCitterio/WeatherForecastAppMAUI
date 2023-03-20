using System.Text.Json.Serialization;

namespace AppMeteoMAUI.Model
{
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
        [JsonPropertyName("hourly")]
        public Hourly Hourly { get; set; }
    }
}
