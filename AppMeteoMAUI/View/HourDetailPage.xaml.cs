using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class HourDetailPage : ContentPage
{
	HourDetailViewModel viewModel;
	public HourDetailPage(ForecastDaily forecastDaily)
	{
		InitializeComponent();
		viewModel = new(forecastDaily);
		BindingContext = viewModel;
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        HourDetailViewModel.PrendiValoriDaVisualizzare();
    }
}