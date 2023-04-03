using CommunityToolkit.Mvvm.ComponentModel;
using AppMeteoMAUI.Model;
using CommunityToolkit.Mvvm.Input;

namespace AppMeteoMAUI.ViewModel
{
    public partial class PreferitiViewModel : ObservableObject
    {
        [ObservableProperty]
        List<Preferiti> favorites;

        public PreferitiViewModel()
        {
            favorites = new();
        }

        [RelayCommand]
        private async void GetFavorites()
        {
            Favorites = await App.PreferitiRepo.GetAllPreferiti();
        }
    }
}
