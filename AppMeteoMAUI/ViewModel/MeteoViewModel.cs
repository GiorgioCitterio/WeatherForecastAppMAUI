using AppMeteoMAUI.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace AppMeteoMAUI.ViewModel
{
    public partial class MeteoViewModel : ObservableObject
    {
        public IGeolocation geolocation;
        static HttpClient client;
        static readonly OpenWeatherMapStore openWeatherMapStore = GetDataFromStore();
        static readonly string openWeatherMapKey = openWeatherMapStore.APIKeyValue;
        MyLocation myCurrentLocation;
        public ObservableCollection<MyLocation> Locations { get; set; }
        [ObservableProperty]
        string text;
        public MeteoViewModel(IGeolocation geolocation)
        {
            this.geolocation = geolocation;
            Locations = new ObservableCollection<MyLocation>();
        }
        [RelayCommand]
        public async Task GetCurrentLocationAsync()
        {
            Location location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest()
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30),
                });
            }
            if (location == null)
                return;
            myCurrentLocation = await GetLocationFromCoordinateAsync(location);
        }
        public async Task<MyLocation> GetLocationFromCoordinateAsync(Location location)
        {
            string url = $"http://api.openweathermap.org/geo/1.0/reverse?lat={location.Latitude}&lon={location.Longitude}&limit=5&appid={openWeatherMapKey}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                MyLocation? myLocation = await response.Content.ReadFromJsonAsync<MyLocation>();
                return myLocation;
            }
            return new();
        }

        [RelayCommand]
        public void CercaLocalita()
        {
            string cittaDaCercare = Text.ToString();
        }
        public static OpenWeatherMapStore GetDataFromStore()
        {
            string keyStorePath = "../../../../../../../WeatherMapStore//MyWeatherApiKey.json";
            string store = File.ReadAllText(keyStorePath);
            OpenWeatherMapStore? openWeatherMapStore = JsonSerializer.Deserialize<OpenWeatherMapStore>(store);
            return openWeatherMapStore ?? new OpenWeatherMapStore();
        }
    }
}
