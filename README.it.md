# App per le previsioni del meteo MAUI

![GitHub all releases](https://img.shields.io/github/downloads/GiorgioCitterio/WeatherForecastAppMAUI/total)
![GitHub](https://img.shields.io/github/license/GiorgioCitterio/WeatherForecastAppMAUI)
![GitHub deployments](https://img.shields.io/github/deployments/GiorgioCitterio/WeatherForecastAppMAUI/github-pages)
![GitHub repo size](https://img.shields.io/github/repo-size/GiorgioCitterio/WeatherForecastAppMAUI)
![GitHub Repo stars](https://img.shields.io/github/stars/GiorgioCitterio/WeatherForecastAppMAUI)

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white)
![Android](https://img.shields.io/badge/Android-3DDC84?style=for-the-badge&logo=android&logoColor=white)

<a href="https://github.com/GiorgioCitterio/WeatherForecastAppMAUI/blob/master/README.md">README.us 🇺🇸</a>

---

## Indice
- <a href="#appoverview">Panoramica dell'app</a>
- <a href="#systemreq">Requisiti di sistema</a>
- <a href="#installation">Come installare l'app</a>
- <a href="#features">Funzionalità</a>
  - <a href="#setdefloc">Impostazione della località predefinita</a>
  - <a href="#discurloc">Visualizzazione della posizione corrente</a>
  - <a href="#searchforw">Ricerca delle previsioni del tempo di ogni città</a>
  - <a href="#lightdarktheme">Tema chiaro e scuro</a>
  - <a href="#temperatureschart">Visualizzazione del grafico a barre delle temperature giornaliere</a>
  - <a href="#favorites">Aggiunta delle città ai preferiti</a>
- <a href="#mauiversion">Versione di .NET MAUI</a>
- <a href="#nuget">Pacchetti NuGet</a>
- <a href="#gifs">Video dell'applicazione</a>

## Panoramica dell'app <a name="appoverview"></a>

L'app mobile è sviluppata utilizzando il framework .NET MAUI, e fornisce agli utenti informazioni sulle previsioni del tempo per diverse città. L'app offre varie funzionalità, come l'impostazione di una località predefinita, la visualizzazione della posizione corrente utilizzando il GPS, la ricerca delle previsioni del tempo di qualsiasi città, il supporto al tema chiaro/scuro basato sulle impostazioni di sistema e la possibilità di visualizzare un grafico a barre delle temperature giornaliere. Inoltre, gli utenti possono aggiungere città ai preferiti e accedere facilmente alle relative previsioni. Attualmente, l'app è disponibile per la piattaforma Android.

## Requisiti di sistema <a name="systemreq"></a>
Dispositivo Android con sistema operativo Android (versione 9 o successiva)

## Installazione <a name="installation"></a>

Per installare l'app sul tuo dispositivo Android, segui [questa guida](https://github.com/GiorgioCitterio/WeatherForecastAppMAUI/wiki).

## Funzionalità <a name="features"></a>

L'app per le previsioni del meteo offre le seguenti funzionalità:

### 1. Impostazione della località predefinita <a name="setdefloc"></a>

- Gli utenti possono impostare una località predefinita in modo che l'app mostri le previsioni del tempo di quella località all'avvio.
- Questa funzionalità consente agli utenti di accedere rapidamente alle informazioni sulle condizioni meteorologiche della loro località preferita senza dover effettuare una ricerca manuale.

### 2. Visualizzazione della posizione corrente <a name="discurloc"></a>
- L'app utilizza il GPS per determinare la posizione corrente dell'utente.
- All'avvio dell'app, vengono visualizzate le previsioni del tempo della posizione corrente.
- Questa funzionalità fornisce agli utenti informazioni meteorologiche in tempo reale sulla loro posizione corrente.

### 3. Ricerca delle previsioni del tempo <a name="searchforw"></a>
- Gli utenti possono cercare le previsioni del tempo di qualsiasi città.
- Inserendo il nome della città nella barra di ricerca, gli utenti possono ottenere le informazioni sulle condizioni meteorologiche per quella specifica località.
- Questa funzionalità consente agli utenti di accedere alle previsioni del tempo di varie città in tutto il mondo.

### 4. Tema chiaro e scuro <a name="lightdarktheme"></a>
- L'app supporta sia il tema chiaro che il tema scuro basati sulle impostazioni di sistema.
- Gli utenti possono godere di un'interfaccia utente visivamente accattivante e confortevole in base al tema preferito.

### 5. Visualizzazione del grafico a barre delle temperature giornaliere <a name="temperatureschart"></a>
- L'app fornisce un grafico a barre che visualizza le temperature giornaliere per una località selezionata.
- Gli utenti possono comprendere facilmente le tendenze e le variazioni di temperatura per ogni giorno.
- Questa funzionalità consente agli utenti di pianificare le proprie attività in base alle condizioni meteorologiche.

### 6. Aggiunta delle città ai preferiti <a name="favorites"></a>
- Gli utenti possono aggiungere città alla lista dei preferiti per accedere rapidamente alle previsioni del tempo di queste località.
- La funzionalità dei preferiti consente agli utenti di visualizzare comodamente le informazioni meteorologiche delle località frequentemente visitate o preferite.

## Versione di .NET MAUI <a name="mauiversion"></a>

L'app è stata sviluppata utilizzando [.NET MAUI 7](https://learn.microsoft.com/en-us/dotnet/maui/whats-new/dotnet-7?view=net-maui-7.0). Questa versione di .NET MAUI offre le ultime funzionalità e miglioramenti per la creazione di applicazioni mobili multipiattaforma.

### Pacchetti NuGet <a name="nuget"></a>

L'app utilizza i seguenti pacchetti NuGet:
- **CommunityToolkit.Mvvm**: Questa libreria Microsoft semplifica l'implementazione dell'architettura Model-View-ViewModel (MVVM) nell'app.
- **sqlite-net-pc**: Questa libreria SQLite-net viene utilizzata per gestire le operazioni del database SQLite nell'app.
- **SQLitePCLRaw.bundle_green**: Questa libreria di Eric Sink fornisce i componenti necessari per la funzionalità del database SQLite.
- **Syncfusion.Maui.Charts**: Questa libreria Syncfusion viene utilizzata per creare il grafico a barre delle temperature giornaliere nell'app.

## Video dell'app <a name="gifs"></a>

<img src="gifs/app_start.gif" width=250px></img>

<img src="gifs/search_city.gif" width=250px></img>

<img src="gifs/favourites.gif" width=250px></img>

<img src="gifs/settings.gif" width=250px></img>
