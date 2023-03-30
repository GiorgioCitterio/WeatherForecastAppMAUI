using AppMeteoMAUI.ViewModel;
namespace AppMeteoMAUI;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();
		MainPage = new AppShell();
	}
}
