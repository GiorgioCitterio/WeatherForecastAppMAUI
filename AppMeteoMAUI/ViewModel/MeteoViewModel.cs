using AppMeteoMAUI.Model;
using AppMeteoMAUI.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

namespace AppMeteoMAUI.ViewModel
{
    public partial class MeteoViewModel : ObservableObject
    {
        [ObservableProperty]
        string text;
        static List<ForecastDaily> forecastDailies = new List<ForecastDaily>();
        public ObservableCollection<CurrentForecast> currentForecast { get; set; }
        static HttpClient? client = new HttpClient();

        public MeteoViewModel()
        {
            //MyProxy.HttpClientProxySetup(out client);
            currentForecast = new ObservableCollection<CurrentForecast>();
            //PrendiPosizionePredefinita();
        }

        private async void PrendiPosizionePredefinita()
        {
            string path = "../../../AppMeteoMAUI/AppMeteoMAUI/Preferences/UltimaPosizioneSalvata.json";
            string content = File.ReadAllText(path);
            string city = JsonSerializer.Deserialize<string>(content);
            (double? lat, double? lon)? geo = await GeoCod(city);
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat}&longitude={geo?.lon}&models=best_match&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset&timeformat=unixtime&forecast_days=7&timezone=Europe%2FBerlin";
            await StampaDatiAsync(urlAdd);
        }
        [RelayCommand]
        async Task GoToDetails(CurrentForecast currentForecast)
        {
            if (currentForecast == null)
                return;

            await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
            {
                {"CurrentForecast", currentForecast }
            });
        }

        [RelayCommand]
        public async Task GetCurrentLocation()
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&models=best_match&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset&timeformat=unixtime&forecast_days=7&timezone=Europe%2FBerlin";
            await StampaDatiAsync(urlAdd);
        }

        [RelayCommand]
        public async Task CercaLocalita()
        {
            string city = Text;
            (double? lat, double? lon)? geo = await GeoCod(city);
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat}&longitude={geo?.lon}&models=best_match&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset&timeformat=unixtime&forecast_days=7&timezone=Europe%2FBerlin";
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
                    return (geocodingResult.Results[0].Latitude, geocodingResult.Results[0].Longitude);
                }
            }
            return null;
        }
        public async Task StampaDatiAsync(FormattableString urlAddUnformattable)
        {
            string urlAdd = FormattableString.Invariant(urlAddUnformattable);
            var response = await client.GetAsync(urlAdd);
            if (response.IsSuccessStatusCode)
            {
                ForecastDaily forecastDaily = await response.Content.ReadFromJsonAsync<ForecastDaily>();
                if (forecastDaily != null)
                {
                    forecastDailies.Add(forecastDaily);
                    for (int i = 0; i < forecastDailies.Count; i++)
                    {
                        currentForecast.Add(new CurrentForecast() { Temperature2mMax = forecastDailies[i].Daily.Temperature2mMax[i], Temperature2mMin = forecastDailies[i].Daily.Temperature2mMin[i], ImageUrl = "mirino.svg" });
                    }
                }
            }
        }
    }
}
