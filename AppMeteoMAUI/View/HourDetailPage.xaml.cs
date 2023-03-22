using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class HourDetailPage : ContentPage
{
	public HourDetailPage(HourDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
    }
}