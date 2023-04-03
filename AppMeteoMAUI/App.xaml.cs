using AppMeteoMAUI.Service;
namespace AppMeteoMAUI;

public partial class App : Application
{
    public static PreferitiRepository PreferitiRepo { get; set; }
    public App(PreferitiRepository repo)
	{
		InitializeComponent();
		MainPage = new AppShell();
        PreferitiRepo = repo;
    }
}
