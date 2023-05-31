namespace AppMeteoMAUI.ViewModel
{
    public partial class MeteoViewModel : ObservableObject
    {
        [ObservableProperty] private string text;
        [ObservableProperty] private double temperatura;
        [ObservableProperty] private string city;
        [ObservableProperty] private ImageSource icona;
        public ObservableCollection<ForecastDaily> ForecastDailiesCollection { get; set; }
        public ObservableCollection<ForecastDaily> ForecastHoursCollection { get; set; }
        private static HttpClient? client = new();
        private string _result;

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
                    _result = await App.Current.MainPage.DisplayPromptAsync("Inserire la posizione predefinita", "");
                } while (_result == null || _result.Length == 0);
                _result = _result.TrimEnd();
                _result = _result.TrimStart();
                if (_result != null)
                {
                    posizione.posizionePredefinita = _result;
                }
                await JsonSerializer.SerializeAsync(fileStream, posizione, options);
                await fileStream.DisposeAsync();
            }
            string fileJson = File.ReadAllText(path);
            PosizionePredefinita pos = JsonSerializer.Deserialize<PosizionePredefinita>(fileJson);
            (double? lat, double? lon)? geo = await GeoCod(pos.posizionePredefinita);
            FormattableString urlAdd = $"https://api.open-meteo.com/v1/forecast?latitude={geo?.lat}&longitude={geo?.lon}&&hourly=temperature_2m,relativehumidity_2m,weathercode,windspeed_10m,winddirection_10m,apparent_temperature,direct_radiation,precipitation_probability,precipitation,uv_index,visibility&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset,apparent_temperature_max,apparent_temperature_min&current_weather=true&timeformat=unixtime&forecast_days=7&timezone=auto";
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
            await App.Current.MainPage.Navigation.PushAsync(new DetailsPage(forecastDaily));
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
            (string, ImageSource) datiImmagine;
            CurrentForecast1Day currentForecast1Day = new();
            CurrentForecast objCur = new();
            DateTime now = DateTime.Now;
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
                        //prende i 7 giorni della settimana
                        for (int i = 0; i < fd.Time.Count; i++)
                        {
                            datiImmagine  = Convertitors.WMOCodesIntIT(fd.Weathercode[i]);
                            objCur = new()
                            {
                                Temperature2mMax = fd.Temperature2mMax[i],
                                Temperature2mMin = fd.Temperature2mMin[i],
                                Data = Convertitors.UnixTimeStampToDateTime(fd.Time[i]),
                                DescMeteo = datiImmagine.Item1,
                                ImageUrl = datiImmagine.Item2,
                                GiornoDellaSettimana = i,
                                ApparentTemperatureMax = fd.ApparentTemperatureMax[i],
                                ApparentTemperatureMin = fd.ApparentTemperatureMin[i],
                            };
                            ForecastDailiesCollection.Add(new ForecastDaily() { CurrentForecast = objCur, Daily = fd, Hourly = forecastDaily.Hourly });
                        }
                        Temperatura = forecastDaily.CurrentWeather.Temperature;
                        DateTime? alba = Convertitors.UnixTimeStampToDateTime(fd.Sunrise[0]);
                        DateTime? tramonto = Convertitors.UnixTimeStampToDateTime(fd.Sunset[0]);
                        (string, ImageSource) currentIcon = Convertitors.WMOCodesIntIT(forecastDaily.CurrentWeather.Weathercode);
                        if (currentIcon.Item1 == "cielo sereno" && (now > tramonto || now.Hour == 0 || now < alba))
                        {
                            currentIcon.Item2 = ImageSource.FromFile("clear_night.svg");
                        }
                        else if (currentIcon.Item1 == "limpido" && (now > tramonto || now.Hour == 0 || now < alba))
                        {
                            currentIcon.Item2 = ImageSource.FromFile("extreme_night.svg");
                        }
                        Icona = currentIcon.Item2;
                        
                        //prende le 24 ore del giorno corrente
                        var fdHour = forecastDaily.Hourly;
                        for (int i = 0; i < 24; i++)
                        {
                            datiImmagine = Convertitors.WMOCodesIntIT(fdHour.Weathercode[i]);
                            currentForecast1Day = new()
                            {
                                Temperature2m = fdHour.Temperature2m[i],
                                Time = Convertitors.UnixTimeStampToDateTime(fdHour.Time[i]),
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
