using System.Text.Json.Serialization;

namespace AppMeteoMAUI.Model
{
    public class OpenWeatherMapStore
    {
        [JsonPropertyName("api_key")]
        public string APIKeyValue { get; set; } = string.Empty;
    }
}
