using AppMeteoMAUI.ViewModel;
using LiveChartsCore.Defaults;

namespace AppMeteoMAUI.View;

public partial class DetailsPage : ContentPage
{
    private DetailsPageViewModel viewModel;
	public DetailsPage(ForecastDaily forecastDaily)
    {
		InitializeComponent();
        viewModel = new DetailsPageViewModel(forecastDaily);
		BindingContext = viewModel;
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DetailsPageViewModel.StampaDati();
    }
}