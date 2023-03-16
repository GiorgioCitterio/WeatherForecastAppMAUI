using AppMeteoMAUI.View;
namespace AppMeteoMAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
        Routing.RegisterRoute(nameof(Settings), typeof(Settings));
    }
}
