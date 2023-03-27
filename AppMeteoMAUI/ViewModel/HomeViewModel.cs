using CommunityToolkit.Mvvm.Input;

namespace AppMeteoMAUI.ViewModel
{
    public partial class HomeViewModel
    {
        [RelayCommand]
        static async void Cod()
        {
            await Launcher.Default.OpenAsync("https://github.com/GiorgioCitterio/AppMeteoMAUI");
        }
        [RelayCommand]
        static async void Issue()
        {
            await Launcher.Default.OpenAsync("https://github.com/GiorgioCitterio/AppMeteoMAUI/issues");
        }
    }
}
