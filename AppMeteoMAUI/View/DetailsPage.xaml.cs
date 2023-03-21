using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(HourDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}