using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class TabBarPageHome : Shell
{
	public TabBarPageHome(MeteoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}