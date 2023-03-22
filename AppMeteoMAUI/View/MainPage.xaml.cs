using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class MainPage : ContentPage
{
	public MainPage(MeteoViewModel viewModel)
	{
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = viewModel;
    }
}
