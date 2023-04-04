using CommunityToolkit.Mvvm.ComponentModel;
using AppMeteoMAUI.Model;
using CommunityToolkit.Mvvm.Input;
using AppMeteoMAUI.View;

namespace AppMeteoMAUI.ViewModel
{
    public partial class PreferitiViewModel : ObservableObject
    {
        [ObservableProperty]
        List<Preferiti> favorites;

        public PreferitiViewModel()
        {
            favorites = new();
            GetFavorites();
        }

        private async void GetFavorites()
        {
            Favorites = await App.PreferitiRepo.GetAllPreferiti();
        }

        [RelayCommand]
        private async void RimuoviPreferito(Preferiti preferito)
        {
            await App.PreferitiRepo.DeletePreferito(preferito);
            await App.Current.MainPage.DisplayAlert("Città rimossa:", preferito.CityName, "OK");
            GetFavorites();
        }

        [RelayCommand]
        private async Task GoToForecast(Result result)
        {
            await Shell.Current.GoToAsync(nameof(MainPage), false);
        }
    }
}
