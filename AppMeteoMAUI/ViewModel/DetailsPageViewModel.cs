using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System.Linq;

namespace AppMeteoMAUI.ViewModel
{
    public partial class DetailsPageViewModel : ObservableObject
    {
        [ObservableProperty] public static ForecastDaily forecast = new();
        [ObservableProperty] private static DateTime? alba;
        [ObservableProperty] private static DateTime? tramonto;
        [ObservableProperty] private static string descrizioneGiornata;
        [ObservableProperty] public static List<ForecastDaily> currentForecast;
        //public ObservableCollection<ForecastDaily> currentForecast { get; set; }
        public static ObservableCollection<DateTimePoint> Data { get; set; }
        private static double windMedio;
        private static double tempMedia;

        public DetailsPageViewModel(ForecastDaily fore)
        {
            //currentForecast = new ObservableCollection<ForecastDaily>();
            CurrentForecast = new();
            forecast = fore;
            Data = new ObservableCollection<DateTimePoint>(currentForecast.Dati.Select(pair => {
                return new DateTimePoint(pair.Key, pair.Value);
            }));
        }

        public static void StampaDati()
        {
            if (forecast.Hourly != null)
            {
                var fd = forecast.Hourly;
                int giorno = (int)forecast.CurrentForecast.GiornoDellaSettimana;
                alba = Convertitors.UnixTimeStampToDateTime(forecast.Daily.Sunrise[giorno]);
                tramonto = Convertitors.UnixTimeStampToDateTime(forecast.Daily.Sunset[giorno]);
                for (int i = 0; i < 24; i++)
                {
                    int index = giorno * 24 + i;
                    (string, ImageSource) datiImmagine = Convertitors.WMOCodesIntIT(fd.Weathercode[index]);
                    CurrentForecast1Day objCur = new()
                    {
                        Temperature2m = fd.Temperature2m[index],
                        ApparentTemperature = fd.ApparentTemperature[index],
                        DescMeteo = datiImmagine.Item1,
                        ImageUrl = datiImmagine.Item2,
                        Time = Convertitors.UnixTimeStampToDateTime(fd.Time[index]),
                        VelVento = fd.Windspeed10m[index],
                        DirVento = Convertitors.ConvertWindDirectionToString(fd.Winddirection10m[index]),
                        OraDelGiorno = i,
                        Precipitation = fd.Precipitation[index],
                        PrecipitationProbability = fd.PrecipitationProbability[index],
                        Relativehumidity2m = fd.Relativehumidity2m[index],
                        UvIndex = fd.UvIndex[index],
                        DirectRadiation = fd.DirectRadiation[index],
                        Visibility = fd.Visibility[index]
                    };
                    if (objCur.DescMeteo == "cielo sereno" && (objCur.Time > tramonto || objCur.Time.Value.Hour == 0 ||objCur.Time < alba))
                    {
                        objCur.ImageUrl = ImageSource.FromFile("clear_night.svg");
                    }
                    else if (objCur.DescMeteo == "limpido" && (objCur.Time > tramonto || objCur.Time.Value.Hour == 0 || objCur.Time < alba))
                    {
                        objCur.ImageUrl = ImageSource.FromFile("extreme_night.svg");
                    }
                    var f = new ForecastDaily();
                    f.CurrentForecast1Day = objCur;
                    f.Dati.Add((DateTime)objCur.Time, objCur.Temperature2m);
                    currentForecast.Add(f);
                    windMedio += (double)objCur.VelVento;
                    tempMedia += objCur.Temperature2m;                
                }
                windMedio /= 24;
                tempMedia /= 24;
                descrizioneGiornata = DayDescription(tempMedia, windMedio);
            }
        }

        #region Metodi Aggiuntivi

        private static string DayDescription(double? degree, double? windspeed)
        {
            if (windspeed > 5)
            {
                switch (windspeed)
                {
                    case double ws when ws <= 15:
                        switch (degree)
                        {
                            case double d when d < -30:
                                return "Giornata nordica, brezza leggera";
                            case double d when d > -20 && d <= -10:
                                return "Giornata gelida, brezza leggera";
                            case double d when d > -10 && d <= 0:
                                return "Giornata invernale, brezza leggera";
                            case double d when d > 0 && d <= 10:
                                return "Giornata fredda, brezza leggera";
                            case double d when d > 10 && d <= 20:
                                return "Giornata temperata, brezza leggera";
                            case double d when d > 20 && d <= 30:
                                return "Giornata afosa, brezza leggera";
                            case double d when d > 30:
                                return "Giornata torrida, brezza leggera";
                            default:
                                return null;
                        }
                    case double ws when ws > 15 && ws <= 30:
                        switch (degree)
                        {
                            case double d when d < -30:
                                return "Giornata nordica, vento moderato";
                            case double d when d > -20 && d <= -10:
                                return "Giornata gelida, vento moderato";
                            case double d when d > -10 && d <= 0:
                                return "Giornata invernale, vento moderato";
                            case double d when d > 0 && d <= 10:
                                return "Giornata fredda, vento moderato";
                            case double d when d > 10 && d <= 20:
                                return "Giornata temperata, vento moderato";
                            case double d when d > 20 && d <= 30:
                                return "Giornata afosa, vento moderato";
                            case double d when d > 30:
                                return "Giornata torrida, vento moderato";
                            default:
                                return null;
                        }
                    case double ws when ws > 30 && ws <= 50:
                        switch (degree)
                        {
                            case double d when d < -30:
                                return "Giornata nordica, vento fresco";
                            case double d when d > -20 && d <= -10:
                                return "Giornata gelida, vento fresco";
                            case double d when d > -10 && d <= 0:
                                return "Giornata invernale, vento fresco";
                            case double d when d > 0 && d <= 10:
                                return "Giornata fredda, vento fresco";
                            case double d when d > 10 && d <= 20:
                                return "Giornata temperata, vento fresco";
                            case double d when d > 20 && d <= 30:
                                return "Giornata afosa, vento fresco";
                            case double d when d > 30:
                                return "Giornata torrida, vento fresco";
                            default:
                                return null;
                        }
                    case double ws when ws > 50 && ws <= 70:
                        switch (degree)
                        {
                            case double d when d < -30:
                                return "Giornata nordica, vento forte";
                            case double d when d > -20 && d <= -10:
                                return "Giornata gelida, vento forte";
                            case double d when d > -10 && d <= 0:
                                return "Giornata invernale, vento forte";
                            case double d when d > 0 && d <= 10:
                                return "Giornata fredda, vento forte";
                            case double d when d > 10 && d <= 20:
                                return "Giornata temperata, vento forte";
                            case double d when d > 20 && d <= 30:
                                return "Giornata afosa, vento forte";
                            case double d when d > 30:
                                return "Giornata torrida, vento forte";
                            default:
                                return null;
                        }
                    case double ws when ws > 70 && ws <= 90:
                        switch (degree)
                        {
                            case double d when d < -30:
                                return "Giornata nordica, burrasca";
                            case double d when d > -20 && d <= -10:
                                return "Giornata gelida, burrasca";
                            case double d when d > -10 && d <= 0:
                                return "Giornata invernale, burrasca";
                            case double d when d > 0 && d <= 10:
                                return "Giornata fredda, tempesta";
                            case double d when d > 10 && d <= 20:
                                return "Giornata temperata, burrasca";
                            case double d when d > 20 && d <= 30:
                                return "Giornata afosa, burrasca";
                            case double d when d > 30:
                                return "Giornata torrida, burrasca";
                            default:
                                return null;
                        }
                    case double ws when ws > 90 && ws <= 110:
                        switch (degree)
                        {
                            case double d when d < -30:
                                return "Giornata nordica, tempesta";
                            case double d when d > -20 && d <= -10:
                                return "Giornata gelida, tempesta";
                            case double d when d > -10 && d <= 0:
                                return "Giornata invernale, tempesta";
                            case double d when d > 0 && d <= 10:
                                return "Giornata fredda, tempesta";
                            case double d when d > 10 && d <= 20:
                                return "Giornata temperata, tempesta";
                            case double d when d > 20 && d <= 30:
                                return "Giornata afosa, tempesta";
                            case double d when d > 30:
                                return "Giornata torrida, tempesta";
                            default:
                                return null;
                        }
                    case double ws when ws > 110:
                        switch (degree)
                        {
                            case double d when d < -30:
                                return "Giornata nordica, uragano";
                            case double d when d > -20 && d <= -10:
                                return "Giornata gelida, uragano";
                            case double d when d > -10 && d <= 0:
                                return "Giornata invernale, uragano";
                            case double d when d > 0 && d <= 10:
                                return "Giornata fredda, uragano";
                            case double d when d > 10 && d <= 20:
                                return "Giornata temperata, uragano";
                            case double d when d > 20 && d <= 30:
                                return "Giornata afosa, uragano";
                            case double d when d > 30:
                                return "Giornata torrida, uragano";
                            default:
                                return null;
                        }
                    default:
                        return null;
                }
            }
            else
            {
                switch (degree)
                {
                    case double d when d < -30:
                        return "Giornata nordica";
                    case double d when d > -20 && d <= -10:
                        return "Giornata gelida";
                    case double d when d > -10 && d <= 0:
                        return "Giornata invernale";
                    case double d when d > 0 && d <= 10:
                        return "Giornata fredda";
                    case double d when d > 10 && d <= 20:
                        return "Giornata temperata";
                    case double d when d > 20 && d <= 30:
                        return "Giornata afosa";
                    case double d when d > 30:
                        return "Giornata torrida";
                    default:
                        return null;
                }
            }
        }

        #endregion

        #region Pagina Dettagli
        [RelayCommand]
        private async Task GoToDetailsHour(ForecastDaily forecastDaily)
        {
            if (forecastDaily == null)
                return;
            await App.Current.MainPage.Navigation.PushAsync(new HourDetailPage(forecastDaily));
        }
        #endregion

        #region Grafico
        public ISeries[] Series { get; set; } = {
            new LineSeries<DateTimePoint>() {
                TooltipLabelFormatter = (point) => $"{new DateTime((long) point.SecondaryValue):dd/MM/yy HH:mm:ss} » {point.PrimaryValue}",
                LineSmoothness = 0,
                Values = Data,
                Stroke = new SolidColorPaint(SKColors.MediumPurple) { StrokeThickness = 6 },
                GeometryStroke = new SolidColorPaint(SKColors.MediumPurple) { StrokeThickness = 6 },
                Fill = new SolidColorPaint(SKColors.MediumPurple.WithAlpha(100))
            }
        };

        public Axis[] XAxes { get; set; } = {
            new Axis() {
                Labeler = value => new DateTime((long) value).ToString("dd/MM/yy HH:mm:ss"),
                LabelsRotation = 20,
                UnitWidth = TimeSpan.FromSeconds(1).Ticks,
                MinStep = TimeSpan.FromSeconds(1).Ticks,
                Name = "Orario",
                NamePaint = new SolidColorPaint(SKColors.White),
                LabelsPaint = new SolidColorPaint(SKColors.White)
            }
        };

        public Axis[] YAxes { get; set; } = {
            new Axis() {
                Name = "Valore",
                NamePaint = new SolidColorPaint(SKColors.White),
                LabelsPaint = new SolidColorPaint(SKColors.White)

            }
        };
        #endregion
    }
}
