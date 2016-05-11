using System;
using System.Linq;
using SenseHat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace TestInterface
{
    public sealed partial class HumidityPage : Page
    {
        private TestInterface.App app = Application.Current as TestInterface.App;

        public HumidityPage()
        {
            this.InitializeComponent();
            //Removing Legend Item from the Chart
            HumChart.LegendItems.Clear();            
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
            var humi = data.Humidity;
            var items = from r in AppData.SensorData
                        select new
                        {
                            Humidity = r.Humidity,
                            DTReading = r.Date.ToString("HH:mm:ss")
                        };

            app.MaxHumi = items.Select(i => i.Humidity).Max();
            app.MinHumi = items.Select(i => i.Humidity).Min();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                btnCurrentHumi.Content = String.Format("Relative\nHumidity:\n{0:f2} %", humi);
                btnMaxHumi.Content = "Maximal\nMeasured\nHumidity:\n" + string.Format("{0:f2} %", app.MaxHumi);
                btnMinHumi.Content = "Minimal\nMeasured\nHumidity:\n" + string.Format("{0:f2} %", app.MinHumi);
                (HumChart.Series[0] as LineSeries).ItemsSource = items;
            });
        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

    }
}
