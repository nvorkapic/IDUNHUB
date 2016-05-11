using System;
using System.Linq;
using SenseHat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace TestInterface
{
    public sealed partial class PressurePage : Page
    {
        private TestInterface.App app = Application.Current as TestInterface.App;
        public PressurePage()
        {
            this.InitializeComponent();
            PressChart.LegendItems.Clear();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AppData.SensorReader.Tick += SensorReader_Tick;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            AppData.SensorReader.Tick -= SensorReader_Tick;
        }

        private async void SensorReader_Tick(SensorReader reader, SensorData data)
        {
            var press = data.Pressure;
            var items = from r in AppData.SensorData
                        select new
                        {
                            Pressure = r.Pressure,
                            DTReading = r.Date.ToString("HH:mm:ss")
                        };

            app.MaxPres = items.Select(i => i.Pressure).Max();
            app.MinPres = items.Select(i => i.Pressure).Min();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                btnMaxPres.Content = "Maximal\nMeasured\nPressure:\n" + string.Format("{0:f2} hPa", app.MaxPres);
                btnMinPres.Content = "Minimal\nMeasured\nPressure:\n" + string.Format("{0:f2} hPa", app.MinPres);
                btnCurrentPress.Content = String.Format("Pressure:\n{0:f2} hPa", press);
                (PressChart.Series[0] as LineSeries).ItemsSource = items;
            });
        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }
    }
}
