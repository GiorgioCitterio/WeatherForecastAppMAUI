namespace AppMeteoMAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
        Routing.RegisterRoute(nameof(Settings), typeof(Settings));
        Routing.RegisterRoute(nameof(HourDetailPage), typeof(HourDetailPage));
        Routing.RegisterRoute(nameof(SearchView), typeof(SearchView));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}
