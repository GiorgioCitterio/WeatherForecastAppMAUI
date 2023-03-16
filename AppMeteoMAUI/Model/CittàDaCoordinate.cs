using System.Text.Json.Serialization;

namespace AppMeteoMAUI.Model
{
    public class CittàDaCoordinate
    {
        [JsonPropertyName("city")]
        public string City { get; set; }
    }
}
