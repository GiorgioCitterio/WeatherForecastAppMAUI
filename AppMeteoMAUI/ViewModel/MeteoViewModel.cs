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
        [ObservableProperty]
        double temperatura;
        static HttpClient? client = new HttpClient();
        public MeteoViewModel()
        {
            currentForecast = new ObservableCollection<CurrentForecast>();
            PrendiPosizionePredefinita();
        }
        #region Posizione Predefinita
        private async void PrendiPosizionePredefinita()
        {
            string path = FileSystem.AppDataDirectory + "/UltimaPosizioneSalvata.json";
            if (!File.Exists(path))
            {
                FileStream fileStream = File.Create(path);
                var options = new JsonSerializerOptions() { WriteIndented = true };
                PosizionePredefinita posizione = new();
                string result = await App.Current.MainPage.DisplayPromptAsync("Inserire la posizione predefinita", "");
                if (result != null)
                {
                    posizione.posizionePredefinita = result;
                }
                await JsonSerializer.SerializeAsync(fileStream, posizione, options);
                await fileStream.DisposeAsync();
            }
            string fileJson = File.ReadAllText(path);
            PosizionePredefinita pos = JsonSerializer.Deserialize<PosizionePredefinita>(fileJson);
            (double? lat, double? lon)? geo = await GeoCod(pos.posizionePredefinita);
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat}&longitude={geo?.lon}&&hourly=temperature_2m,weathercode,windspeed_10m,winddirection_10m,apparent_temperature,precipitation_probability,precipitation,showers&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset,apparent_temperature_max,apparent_temperature_min&current_weather=true&timeformat=unixtime&forecast_days=7&timezone=auto";
            await StampaDatiAsync(urlAdd);
        }
        #endregion

        #region Pagina Dettagli
        [RelayCommand]
        private async Task GoToDetails(CurrentForecast currentForecast)
        {
            if (currentForecast == null)
                return;

            await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
            {
                {"CurrentForecast", currentForecast }
            });
        }
        #endregion

        #region Pagina Impostazioni
        [RelayCommand]
        private async Task GoToSettings()
        {
            await Shell.Current.GoToAsync(nameof(Settings));
        }
        #endregion

        #region Geolocalizzazione

        [RelayCommand]
        public async Task GetCurrentLocation()
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&&hourly=temperature_2m,weathercode,windspeed_10m,winddirection_10m,apparent_temperature,precipitation_probability,precipitation,showers&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset,apparent_temperature_max,apparent_temperature_min&current_weather=true&timeformat=unixtime&forecast_days=7&timezone=auto";
            await StampaDatiAsync(urlAdd);
        }
        #endregion

        #region Cerca località

        [RelayCommand]
        public async Task CercaLocalita()
        {
            string city = Text;
            (double? lat, double? lon)? geo = await GeoCod(city);
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat}&longitude={geo?.lon}&&hourly=temperature_2m,weathercode,windspeed_10m,winddirection_10m,apparent_temperature,precipitation_probability,precipitation,showers&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset,apparent_temperature_max,apparent_temperature_min&current_weather=true&timeformat=unixtime&forecast_days=7&timezone=auto";
            await StampaDatiAsync(urlAdd);
        }
        #endregion

        #region StampaDati
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
                        currentForecast.Add(new CurrentForecast() { Temperature2mMax = fd.Temperature2mMax[i], Temperature2mMin = fd.Temperature2mMin[i], ImageUrl = "sun_behind_small_cloud.png", Data = UnixTimeStampToDateTime(fd.Time[i])});
                    }
                    Temperatura = forecastDaily.CurrentWeather.Temperature;
                }
            }
        }
        #endregion

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
        static string WMOCodesIntIT(int? code)
        {
            string result = code switch
            {
                0 => "cielo sereno",
                1 => "prevalentemente limpido",
                2 => "parzialmente nuvoloso",
                3 => "coperto",
                45 => "nebbia",
                48 => "nebbia con brina",
                51 => "pioggerellina di scarsa intensità",
                53 => "pioggerellina di moderata intensità",
                55 => "pioggerellina intensa",
                56 => "pioggerellina gelata di scarsa intensità",
                57 => "pioggerellina gelata intensa",
                61 => "pioggia di scarsa intensità",
                63 => "pioggia di moderata intensità",
                65 => "pioggia molto intensa",
                66 => "pioggia gelata di scarsa intensità",
                67 => "pioggia gelata intensa",
                71 => "nevicata di lieve entità",
                73 => "nevicata di media entità",
                75 => "nevicata intensa",
                77 => "granelli di neve",
                80 => "deboli rovesci di pioggia",
                81 => "moderati rovesci di pioggia",
                82 => "violenti rovesci di pioggia",
                85 => "leggeri rovesci di neve",
                86 => "pesanti rovesci di neve",
                95 => "temporale lieve o moderato",
                96 => "temporale con lieve grandine",
                99 => "temporale con forte grandine",
                _ => string.Empty,
            };
            return result;
        }
        #endregion
    }
}
