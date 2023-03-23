using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class TabBarPageHome : Shell
{
	public TabBarPageHome()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }
}