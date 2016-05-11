using System;
using System.Linq;
using SenseHat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace TestInterface
{
    public sealed partial class TemperaturePage : Page
    {
        private TestInterface.App app = Application.Current as TestInterface.App;

        public float ConvertedTemp;

        public TemperaturePage()
        {
            this.InitializeComponent();
            TempChart.LegendItems.Clear();
            var series = TempChart.Series[0] as LineSeries;
            series.DependentRangeAxis = new LinearAxis
            {
                Minimum = 25,
                Maximum = 35,
                Orientation = AxisOrientation.Y,
                Interval = 0.5,
                ShowGridLines = true
            };
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
            ConvertedTemp = data.Temperature;

            var items = from r in AppData.SensorData
                        select new
                        {
                            Temperature = r.Temperature,
                            DTReading = r.Date.ToString("HH:mm:ss.") + r.Date.Millisecond
                        };

            foreach (var temp in items)
            {
                app.MaxTemp = temp.Temperature > app.MaxTemp ? temp.Temperature : app.MaxTemp;
                app.MinTemp = temp.Temperature < app.MinTemp ? temp.Temperature : app.MinTemp;
            }

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                btnCurrentTemp.Content = string.Format("Temperature:\n{0:f2} °C", ConvertedTemp);
                btnMaxVal.Content = "Maximal\nMeasured\nTemperature:\n" + string.Format("{0:f2} °C", app.MaxTemp);
                btnMinVal.Content = "Minimal\nMeasured\nTemperature:\n" + string.Format("{0:f2} °C", app.MinTemp);
                (TempChart.Series[0] as LineSeries).ItemsSource = items;
            });

        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private void onLoadMUnitBtn(object sender, RoutedEventArgs e)
        {
            //btnMUnit.Content = "Measuring\nUnit:\n" + TempUnit;
        }
    }
}
