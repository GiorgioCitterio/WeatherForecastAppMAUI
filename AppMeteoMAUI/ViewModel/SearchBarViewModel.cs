using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AppMeteoMAUI.ViewModel
{
    public class SearchBarViewModel : ObservableObject
    {
        public ObservableCollection<Location> Locations { get; set; }

        [ObservableProperty]
        string localita;
        public SearchBarViewModel()
        {
            Locations = new ObservableCollection<Location>();
        }
        [RelayCommand]
        public void CercaLocalita()
        {
            Location location;
        }
    }
}
