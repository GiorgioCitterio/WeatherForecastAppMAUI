namespace AppMeteoMAUI.View;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
    }
}