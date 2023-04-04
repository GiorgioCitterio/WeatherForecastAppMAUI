using AppMeteoMAUI.Model;
using AppMeteoMAUI.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.PlatformConfiguration;
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
        [ObservableProperty]
        double temperatura;
        [ObservableProperty]
        string city;
        [ObservableProperty]
        ImageSource icona;
        public ObservableCollection<ForecastDaily> ForecastDailiesCollection { get; set; }
        public ObservableCollection<ForecastDaily> ForecastHoursCollection { get; set; }
        static HttpClient? client = new();
        string result;

        public MeteoViewModel()
        {
            ForecastDailiesCollection = new ObservableCollection<ForecastDaily>();
            ForecastHoursCollection = new ObservableCollection<ForecastDaily>();
            var eseguiPredefinito = Preferences.Get("esegui_predefinito", true);
            var opzioneSelezionata = Preferences.Get("opzione_selezionata", "posizione_predefinita");
            if (eseguiPredefinito)
            {
                if (opzioneSelezionata == "posizione_corrente")
                {
                    GetCurrentLocation();
                }
                else if (opzioneSelezionata == "posizione_predefinita")
                {
                    PrendiPosizionePredefinita();
                }
            }
            else
            {
                string posDaCercare = Preferences.Get("citta_scelta_search", "Cassago Brianza");
                Text = posDaCercare;
                CercaLocalita();
                Preferences.Set("esegui_predefinito", true);
            }
                
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
                do
                {
                    result = await App.Current.MainPage.DisplayPromptAsync("Inserire la posizione predefinita", "");
                } while (result == null || result.Length == 0);
                result = result.TrimEnd();
                result = result.TrimStart();
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
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat}&longitude={geo?.lon}&&hourly=temperature_2m,relativehumidity_2m,weathercode,windspeed_10m,winddirection_10m,apparent_temperature,precipitation_probability,precipitation,showers&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset,apparent_temperature_max,apparent_temperature_min&current_weather=true&timeformat=unixtime&forecast_days=7&timezone=auto";
            await StampaDatiAsync(urlAdd);
            City = pos.posizionePredefinita;
        }
        #endregion

        #region Pagina Dettagli
        [RelayCommand]
        private async Task GoToDetails(ForecastDaily forecastDaily)
        {
            if (forecastDaily == null)
                return;
            DetailsPageViewModel viewModel = new(forecastDaily);
            await App.Current.MainPage.Navigation.PushAsync(new DetailsPage(viewModel));
        }
        #endregion

        #region Pagina Search
        [RelayCommand]
        private async Task GoToSearchPage()
        {
            await Shell.Current.GoToAsync(nameof(SearchView));
        }
        #endregion

        #region MainPage
        [RelayCommand]
        private async Task BackToMainPage()
        {
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }
        #endregion

        #region Geolocalizzazione

        [RelayCommand]
        public async Task GetCurrentLocation()
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&&hourly=temperature_2m,relativehumidity_2m,weathercode,windspeed_10m,winddirection_10m,apparent_temperature,direct_radiation,precipitation_probability,precipitation,uv_index,visibility&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset,apparent_temperature_max,apparent_temperature_min&current_weather=true&timeformat=unixtime&forecast_days=7&timezone=auto";
            await StampaDatiAsync(urlAdd);
            FormattableString formattableString = $"https://api.bigdatacloud.net/data/reverse-geocode-client?latitude={location.Latitude}&longitude={location.Longitude}&localityLanguage=en";
            string urlRecuperaCity = FormattableString.Invariant(formattableString);
            CittàDaCoordinate cittàDaCoordinate = await client.GetFromJsonAsync<CittàDaCoordinate>(urlRecuperaCity);
            City = cittàDaCoordinate.City;
        }
        #endregion

        #region Cerca località

        [RelayCommand]
        public async Task CercaLocalita()
        {
            string city = Text;
            (double? lat, double? lon)? geo = await GeoCod(city);
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat}&longitude={geo?.lon}&&hourly=temperature_2m,relativehumidity_2m,weathercode,windspeed_10m,winddirection_10m,apparent_temperature,direct_radiation,precipitation_probability,precipitation,uv_index,visibility&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset,apparent_temperature_max,apparent_temperature_min&current_weather=true&timeformat=unixtime&forecast_days=7&timezone=auto";
            await StampaDatiAsync(urlAdd);
            City = city;
        }
        #endregion

        #region StampaDati
        public async Task StampaDatiAsync(FormattableString urlAddUnformattable)
        {
            string urlAdd = FormattableString.Invariant(urlAddUnformattable);
            var response = await client.GetAsync(urlAdd);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    ForecastDaily forecastDaily = await response.Content.ReadFromJsonAsync<ForecastDaily>();
                    if (forecastDaily.Daily != null)
                    {
                        var fd = forecastDaily.Daily;
                        ForecastDailiesCollection.Clear();
                        for (int i = 0; i < fd.Time.Count; i++)
                        {
                            (string, ImageSource) datiImmagine = WMOCodesIntIT(fd.Weathercode[i]);
                            CurrentForecast objCur = new()
                            {
                                Temperature2mMax = fd.Temperature2mMax[i],
                                Temperature2mMin = fd.Temperature2mMin[i],
                                Data = UnixTimeStampToDateTime(fd.Time[i]),
                                DescMeteo = datiImmagine.Item1,
                                ImageUrl = datiImmagine.Item2,
                                GiornoDellaSettimana = i,
                                ApparentTemperatureMax = fd.ApparentTemperatureMax[i],
                                ApparentTemperatureMin = fd.ApparentTemperatureMin[i],
                            };
                            ForecastDailiesCollection.Add(new ForecastDaily() { CurrentForecast = objCur, Daily = fd, Hourly = forecastDaily.Hourly });
                        }
                        Temperatura = forecastDaily.CurrentWeather.Temperature;
                        DateTime? alba = UnixTimeStampToDateTime(fd.Sunrise[0]);
                        DateTime? tramonto = UnixTimeStampToDateTime(fd.Sunset[0]);
                        DateTime ora = DateTime.Now;
                        (string, ImageSource) currentIcon = WMOCodesIntIT(forecastDaily.CurrentWeather.Weathercode);
                        if (currentIcon.Item1 == "cielo sereno" && (ora > tramonto || ora.Hour == 0 || ora < alba))
                        {
                            currentIcon.Item2 = ImageSource.FromFile("clear_night.svg");
                        }
                        else if (currentIcon.Item1 == "limpido" && (ora > tramonto || ora.Hour == 0 || ora < alba))
                        {
                            currentIcon.Item2 = ImageSource.FromFile("extreme_night.svg");
                        }
                        Icona = currentIcon.Item2;

                        var fdHour = forecastDaily.Hourly;
                        for (int i = 0; i < 24; i++)
                        {
                            (string, ImageSource) datiImmagine = WMOCodesIntIT(fdHour.Weathercode[i]);
                            CurrentForecast1Day currentForecast1Day = new()
                            {
                                Temperature2m = fdHour.Temperature2m[i],
                                Time = UnixTimeStampToDateTime(fdHour.Time[i]),
                                ImageUrl = datiImmagine.Item2
                            };
                            if (datiImmagine.Item1 == "cielo sereno" && (currentForecast1Day.Time > tramonto || currentForecast1Day.Time.Value.Hour == 0 || currentForecast1Day.Time < alba))
                            {
                                currentForecast1Day.ImageUrl = ImageSource.FromFile("clear_night.svg");
                            }
                            else if (datiImmagine.Item1 == "limpido" && (currentForecast1Day.Time > tramonto || currentForecast1Day.Time.Value.Hour == 0 || currentForecast1Day.Time < alba))
                            {
                                currentForecast1Day.ImageUrl = ImageSource.FromFile("extreme_night.svg");
                            }
                            ForecastHoursCollection.Add(new ForecastDaily() { CurrentForecast1Day = currentForecast1Day });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Errore!", ex.Message, "cancel");
            }
            
        }
        #endregion

        #region Metodi Aggiungitivi
        static async Task<(double? lat, double? lon)?> GeoCod(string city)
        {
            try
            {
                string? cityUrlEncoded = HttpUtility.UrlEncode(city);
                string url = $"https://geocoding-api.open-meteo.com/v1/search?name={cityUrlEncoded}&language=it&count=5";
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
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Errore!", ex.Message, "cancel");
                return null;
            }
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
        static (string, ImageSource) WMOCodesIntIT(int? code)
        {
            return code switch
            {
                0 => ("cielo sereno", ImageSource.FromFile("clear_day.svg")),
                1 => ("limpido", ImageSource.FromFile("partly_cloudy_day.svg")),
                2 => ("annuvolato", ImageSource.FromFile("cloudy.svg")),
                3 => ("coperto", ImageSource.FromFile("extreme_rain.svg")),
                45 => ("nebbia", ImageSource.FromFile("fog.svg")),
                48 => ("brina", ImageSource.FromFile("extreme_fog.svg")),
                51 => ("pioggerella", ImageSource.FromFile("drizzle.svg")),
                53 => ("pioggerella", ImageSource.FromFile("drizzle.svg")),
                55 => ("pioggerella intensa", ImageSource.FromFile("drizzle.svg")),
                56 => ("pioggerella gelata", ImageSource.FromFile("sleet.svg")),
                57 => ("pioggerella gelata", ImageSource.FromFile("extreme_sleet.svg")),
                61 => ("pioggia scarsa", ImageSource.FromFile("drizzle.svg")),
                63 => ("pioggia moderata", ImageSource.FromFile("drizzle.svg")),
                65 => ("pioggia intensa", ImageSource.FromFile("extrene_drizzle.svg")),
                66 => ("pioggia gelata", ImageSource.FromFile("sleet.svg")),
                67 => ("pioggia gelata", ImageSource.FromFile("extreme_sleet.svg")),
                71 => ("nevicata lieve", ImageSource.FromFile("snow.svg")),
                73 => ("nevicata media", ImageSource.FromFile("snow.svg")),
                75 => ("nevicata intensa", ImageSource.FromFile("extreme_snow.svg")),
                77 => ("granelli di neve", ImageSource.FromFile("sleet.svg")),
                80 => ("pioggia debole", ImageSource.FromFile("drizzle.svg")),
                81 => ("pioggia moderata", ImageSource.FromFile("drizzle.svg")),
                82 => ("pioggia violenta", ImageSource.FromFile("extreme_drizzle.svg")),
                85 => ("neve leggera", ImageSource.FromFile("snow.svg")),
                86 => ("neve pesante", ImageSource.FromFile("extreme_snow.svg")),
                95 => ("temporale lieve", ImageSource.FromFile("drizzle.svg")),
                96 => ("temporale grandine", ImageSource.FromFile("sleet.svg")),
                99 => ("temporale grandine", ImageSource.FromFile("extreme_sleet.svg"))
            };
        }
        #endregion

        #region Aggiungi Preferito
        [RelayCommand]
        private async void AggiungiPreferito(string nomeCity)
        {
            (double? lat, double? lon)? geo = await GeoCod(nomeCity);
            Preferiti pref = new()
            {
                CityName = nomeCity,
                Latitude = geo.Value.lat,
                Longitude = geo.Value.lon
            };
            try
            {
                await App.PreferitiRepo.AddPreferito(pref);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Errore!", ex.Message, "OK");
            }
            await App.Current.MainPage.DisplayAlert("Nuova città aggiunta:", nomeCity, "OK");
        }
        #endregion
    }
}
