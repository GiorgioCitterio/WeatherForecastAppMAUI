using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class Home : Shell
{
	public Home()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
		Shell.SetBackgroundColor(this, Color.FromRgb(0, 0, 0));
    }
}