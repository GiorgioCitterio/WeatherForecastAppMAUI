using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using AppMeteoMAUI.Model;

namespace AppMeteoMAUI.ViewModel
{
    public class MeteoViewModel
    {
        static HttpClient client;
        static async Task PrevisioniOpenGeoCoding()
        {
            string city = "Monticello Brianza"; //prendere il testo della label per cercare
            (double? lat, double? lon)? geo = await GeoCod(city);
            string urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat.ToString().Replace(',', '.')}&longitude={geo?.lon.ToString().Replace(',', '.')}&models=best_match&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset&timeformat=unixtime&forecast_days=3&timezone=Europe%2FBerlin";
            var response = await client.GetAsync($"{urlAdd}");
            {
                if (response.IsSuccessStatusCode)
                {
                    ForecastDaily? forecastDaily = await response.Content.ReadFromJsonAsync<ForecastDaily>();
                    JsonSerializerOptions options = new(JsonSerializerDefaults.Web) { WriteIndented = true };
                    Console.WriteLine("Dati ricevuti dall'endpoint remoto:\n" + JsonSerializer.Serialize(forecastDaily, options));
                    var fd = forecastDaily.Daily;
                }
            }
        }
        static async Task<(double? lat, double? lon)?> GeoCod(string city)
        {
            string? cityUrlEncoded = HttpUtility.UrlEncode(city);
            string url = $"https://geocoding-api.open-meteo.com/v1/search?name={cityUrlEncoded}&language=it&count=1";
            HttpResponseMessage responseGeocoding = await client.GetAsync($"{url}");
            if (responseGeocoding.IsSuccessStatusCode)
            {
                GeoCoding? geocodingResult = await responseGeocoding.Content.ReadFromJsonAsync<GeoCoding>();
                if (geocodingResult != null)
                {
                    Console.WriteLine(geocodingResult.Results[0].Latitude + " " + geocodingResult.Results[0].Longitude);
                    return (geocodingResult.Results[0].Latitude, geocodingResult.Results[0].Latitude);
                }
            }
            return null;
        }
        static DateTime? UnixTimeStampToDateTime(double? unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            if (unixTimeStamp != null)
            {
                DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds((double)unixTimeStamp).ToLocalTime();
                return dateTime;
            }
            return null;
        }
    }
}
