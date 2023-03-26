using AppMeteoMAUI.ViewModel;

namespace AppMeteoMAUI.View;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void pos_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(sender == posCorr)
        {
            Preferences.Set("opzione_selezionata", "posizione_corrente");
        }
        else if(sender == posPred)
        {
            Preferences.Set("opzione_selezionata", "posizione_predefinita");
        }
    }
}