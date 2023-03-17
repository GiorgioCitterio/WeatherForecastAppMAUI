using CommunityToolkit.Mvvm.Input;
using AppMeteoMAUI.Model;
using System.Text.Json;

namespace AppMeteoMAUI.ViewModel
{
    public partial class SettingsViewModel
    {
        string result;
        public SettingsViewModel() { }
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
                result = await App.Current.MainPage.DisplayPromptAsync("Inserire la posizione predefinita", "");
            } while (result == null || result.Length == 0);
            if (result != null)
            {
                posizione.posizionePredefinita = result;
            }
            FileStream fileStream = File.Create(path);
            await JsonSerializer.SerializeAsync(fileStream, posizione, options);
            await fileStream.DisposeAsync();
            string fileJson = File.ReadAllText(path);
            PosizionePredefinita pos = JsonSerializer.Deserialize<PosizionePredefinita>(fileJson);
            await App.Current.MainPage.DisplayAlert("Cambiamento avvenuto correttamente", "Nuova posizione predefinita: " + pos.posizionePredefinita, "OK");
        }
    }
}
