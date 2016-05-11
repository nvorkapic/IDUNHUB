using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SenseHat;
using Windows.Security.Credentials;
using TestInterface.Models;
using TestInterface.Pages;

namespace TestInterface
{
    public sealed partial class MainPage : Page
    {
        private TestInterface.App app = Application.Current as TestInterface.App; 
        public string RHumi;
        public string RPres;
        public string RTemp;
        double number;
        double barMax;
        double barCur;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ListViewTest.ItemsSource = new[] { "Loading data..." };
            var templates = await AppData.CloudClient.GetTemplates();
            ListViewTest.ItemsSource = templates.Select(t => new TemplateModel(t));
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
            app.MaxTemp = Math.Max(data.Temperature, app.MaxTemp);
            app.MinTemp = Math.Min(data.Temperature, app.MinTemp);
            app.MaxPres = Math.Max(data.Pressure, app.MaxPres);
            app.MinPres = Math.Min(data.Pressure, app.MinPres);
            app.MaxHumi = Math.Max(data.Humidity, app.MaxHumi);
            app.MinHumi = Math.Min(data.Humidity, app.MinHumi);

            RHumi = String.Format(" Relative Humidity: {0:f2} % ", data.Humidity);
            RPres = String.Format(" Pressure: {0:f2} hPa ", data.Pressure);
            RTemp = string.Format(" Temperature: {0:f2} °C ", data.Temperature);

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                btnHumidity.Content = String.Format("Relative\nHumidity:\n{0:f2} %", data.Humidity);
                btnPressure.Content = String.Format("Pressure:\n{0:f2} hPa", data.Pressure);
                btnTemp.Content = string.Format("Temperature:\n{0:f2} °C", data.Temperature);
            });
        }

        private void btnTemp_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TemperaturePage), null);
        }

        private void btnPressure_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PressurePage), null);
        }

        private void btnHumidity_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HumidityPage), null);
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StatusBar), null); 
        }

        private void btnMCounter_Click(object sender, RoutedEventArgs e)
        {
            //Global number of Servie Calls
            if (app.MaxNrBfrMaintenance > app.currentNrofServiceCalls)
            {
                ++app.currentNrofServiceCalls;
            }
            //Service Call after Current Number of Service Calls reaches its' maximum
            if (app.MaxNrBfrMaintenance == app.currentNrofServiceCalls)
            {
                //Global List with Service Calls
                string Datum = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string maxtemp =" Maximal Temperature: "+string.Format("{0:f2} °C",app.MaxTemp);
                string mintemp =" Minimal Temperature: " + string.Format("{0:f2} °C", app.MinTemp);
                string maxhumi =" Maximal Humidity: " + string.Format("{0:f2} %", app.MaxHumi);
                string minhumi =" Minimal Humidity: " + string.Format("{0:f2} %", app.MinHumi);
                string maxpres =" Maximal Pressure: " + string.Format("{0:f2} hPa", app.MaxPres);
                string minpres =" Minimal Pressure: " + string.Format("{0:f2} hPa", app.MinPres);
                app.ReportForMain.Insert(0, new ReportModel { DTofServiceCall = Datum, SCHumidity = RHumi, SCPressure = RPres, SCTemperature= RTemp, MaxNr = " Maintenance After "+app.currentNrofServiceCalls.ToString()+" Run(s) ",MaxTemperature=maxtemp,MinTemperature=mintemp,MaxPressure=maxpres,MinPressure=minpres,MaxHumidity=maxhumi,MinHumidity=minhumi, Note=" Note: Automatic Insertion " });
                app.currentNrofServiceCalls = 0;
                
                //Local List with Service Calls
                ListViewTest.ItemsSource = app.ReportForMain;

                //Button with Report
                button.Content = "Service Called " + app.ReportForMain.Count + " time(s)";
            }
            //Button with Current Calls Value
            btnMCounter.Content = "Usage Calls:\n" + app.currentNrofServiceCalls + "\nout of\n" + app.MaxNrBfrMaintenance;

            //Progress Bar
            int barMaxint = app.MaxNrBfrMaintenance;
            double barMax = barMaxint;
            ProgBarforStatus.Maximum=barMax;
            int barCurrentint = app.currentNrofServiceCalls;
            double barCur = barCurrentint;
            ProgBarforStatus.Value = barCur;
        }

        private void onLoadMCounter(object sender, RoutedEventArgs e)
        {
            btnMCounter.Content = "Usage Calls:\n" + app.currentNrofServiceCalls + "/" + app.MaxNrBfrMaintenance;
        }

        private void barOnLoad(object sender, RoutedEventArgs e)
        {
            //Progress Bar on Load
            int barMaxint = app.MaxNrBfrMaintenance;
            if (double.TryParse(barMaxint.ToString(), out number))
            {
                barMax = number;
            }
            else
            {
                barMax = 0;
            }
            
            ProgBarforStatus.Maximum = barMax;

            int barCurrentint = app.currentNrofServiceCalls;
            if (double.TryParse(barCurrentint.ToString(), out number))
            {
                barCur = number;
            }
            else
            {
                barCur = 0;
            }

            ProgBarforStatus.Value = barCur;
        }

        private void onLoadList(object sender, RoutedEventArgs e)
        {
            //List on Load
            ListViewTest.ItemsSource = app.ReportForMain;
        }

        private void button_Loaded(object sender, RoutedEventArgs e)
        {
            button.Content = "Service Called "+app.ReportForMain.Count+" time(s)";
        }

        private void btnAAS_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdditionalApps), null);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ConfigMainPage), null);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TestLiveGraph), null);
        }
    }
}
