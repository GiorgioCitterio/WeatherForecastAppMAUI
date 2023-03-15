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
        public ObservableCollection<CurrentForecast> currentForecast { get; set; }
        static HttpClient? client = new HttpClient();
        public MeteoViewModel()
        {
            currentForecast = new ObservableCollection<CurrentForecast>();
            //MyProxy.HttpClientProxySetup(out client);
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
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat}&longitude={geo?.lon}&hourly=temperature_2m,windspeed_1000hPa,winddirection_1000hPa&models=ecmwf_ifs04&daily=temperature_2m_max,temperature_2m_min,sunrise,sunset&timeformat=unixtime&timezone=Europe%2FBerlin";
            await StampaDatiAsync(urlAdd);
        }

        public async Task StampaDatiAsync(FormattableString urlAddUnformattable)
        {
            string urlAdd = FormattableString.Invariant(urlAddUnformattable);
            var response = await client.GetAsync(urlAdd);
            if (response.IsSuccessStatusCode)
            {
                ForecastDaily forecastDaily = await response.Content.ReadFromJsonAsync<ForecastDaily>();
                if (forecastDaily.Daily != null)
                {
                    var fd = forecastDaily.Daily;
                    currentForecast.Clear();
                    for (int i = 0; i < fd.Time.Count; i++)
                    {
                        currentForecast.Add(new CurrentForecast() { Temperature2mMax = fd.Temperature2mMax[i], Temperature2mMin = fd.Temperature2mMin[i], ImageUrl = "sun_behind_small_cloud.png", Data = UnixTimeStampToDateTime(fd.Time[i]) });
                    }
                }
            }
        }
        #region Metodi Aggiungitivi
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
        private static DateTime? UnixTimeStampToDateTime(double? unixTimeStamp)
        {
            if (unixTimeStamp != null)
            {
                DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds((double)unixTimeStamp).ToLocalTime();
                return dateTime;
            }
            return null;
        }
        #endregion
    }
}
