using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI;

public partial class MainPage : ContentPage
{
	public MainPage(MeteoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
