using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class Meteo1Day : ContentPage
{
	public Meteo1Day(Meteo1DayViewModel viewModel)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = viewModel;
    }
}