using AppMeteoMAUI.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Web;

namespace AppMeteoMAUI.ViewModel
{
    public partial class MeteoViewModel : ObservableObject
    {
        [ObservableProperty]
        string text;
        public List<ForecastDaily> forecastDailies { get; set; }
        public ObservableCollection<CurrentForecast> currentForecast { get; set; }
        static HttpClient client = new HttpClient();

        public MeteoViewModel()
        {
            currentForecast = new ObservableCollection<CurrentForecast>();
        }
        [RelayCommand]
        public async Task GetCurrentLocation()
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();
            string urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude.ToString().Replace(',', '.')}&longitude={location.Longitude.ToString().Replace(',', '.')}&models=best_match&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset&timeformat=unixtime&forecast_days=1&timezone=Europe%2FBerlin";
            await StampaDatiAsync(urlAdd);
        }

        [RelayCommand]
        public async Task CercaLocalita()
        {
            string city = Text;
            (double? lat, double? lon)? geo = await GeoCod(city);
            string urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat.ToString().Replace(',', '.')}&longitude={geo?.lon.ToString().Replace(',', '.')}&models=best_match&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset&timeformat=unixtime&forecast_days=3&timezone=Europe%2FBerlin";
            await StampaDatiAsync(urlAdd);
        }
        static async Task<(double? lat, double? lon)?> GeoCod(string city)
        {
            string? cityUrlEncoded = HttpUtility.UrlEncode(city);
            string url = $"https://geocoding-api.open-meteo.com/v1/search?name={cityUrlEncoded}&language=it&count=7";
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
        public async Task StampaDatiAsync(string urlAdd)
        {
            var response = await client.GetAsync(urlAdd);
            if (response.IsSuccessStatusCode)
            {
                ForecastDaily forecastDaily = await response.Content.ReadFromJsonAsync<ForecastDaily>();
                if (forecastDaily != null)
                {
                    forecastDailies.Add(forecastDaily);
                }
            }
            for (int i = 0; i < forecastDailies.Count; i++)
            {
                currentForecast.Add(new CurrentForecast() { Temperature2mMax = forecastDailies[i].Daily.Temperature2mMax[i], Temperature2mMin = forecastDailies[i].Daily.Temperature2mMin[i] });
            }
        }
    }
}
