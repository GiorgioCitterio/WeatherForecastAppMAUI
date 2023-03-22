using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class Meteo1Day : ContentPage
{
	public Meteo1Day()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
    }
}