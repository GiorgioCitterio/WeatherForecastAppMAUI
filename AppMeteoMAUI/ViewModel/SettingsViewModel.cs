namespace AppMeteoMAUI.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        public SettingsViewModel() 
        {
            StatoInizialeRadioBtn();
        }

        private string _result;
        [ObservableProperty] bool posPredStatus;
        [ObservableProperty] bool posCorrStatus;

        [RelayCommand]
        private async void CambiaPosizionePredefinita()
        {
            string path = FileSystem.AppDataDirectory + "/UltimaPosizioneSalvata.json";
            if (!File.Exists(path))
                return;
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
            FileStream fileStream = File.Create(path);
            await JsonSerializer.SerializeAsync(fileStream, posizione, options);
            await fileStream.DisposeAsync();
            string fileJson = File.ReadAllText(path);
            PosizionePredefinita pos = JsonSerializer.Deserialize<PosizionePredefinita>(fileJson);
            await App.Current.MainPage.DisplayAlert("Cambiamento avvenuto correttamente", "Nuova posizione predefinita: " + pos.posizionePredefinita, "OK");
        }
        private void StatoInizialeRadioBtn()
        {
            var opzioneSelezionata = Preferences.Get("opzione_selezionata", "posizione_corrente");
            if (opzioneSelezionata == "posizione_corrente")
            {
                PosCorrStatus = true;
            }
            else if (opzioneSelezionata == "posizione_predefinita")
            {
                PosPredStatus = true;
            }
        }
    }
}
