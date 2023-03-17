using System.Text.Json.Serialization;

namespace AppMeteoMAUI.Model
{
    public class CurrentForecast1Day
    {
        [JsonPropertyName("time")]
        public int Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double Temperature2m { get; set; }
        [JsonPropertyName("apparent_temperature")]
        public double ApparentTemperature { get; set; }
        [JsonPropertyName("image_url")]
        public ImageSource ImageUrl { get; set; }
        [JsonPropertyName("desc_meteo")]
        public string DescMeteo { get; set; }
    }
}
