using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SenseHat;
using TestInterface.Models;

namespace TestInterface
{
    public sealed partial class statusBarSetting : Page
    {
        private TestInterface.App app = Application.Current as TestInterface.App;

        public string RHumi;
        public string RPres;
        public string RTemp;
        int inumber;
        public statusBarSetting()
        {
            this.InitializeComponent();
            keyboard.RegisterTarget(textBoxNote);
            keyboard.RegisterTarget(MaxServiceNr);

            if ((Application.Current as App).DeviceType != App.Device.Iot) { keyboard.Visibility = Visibility.Collapsed; }
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

        private void SensorReader_Tick(SensorReader reader, SensorData data)
        {
            RHumi = String.Format(" Relative Humidity: {0:f2} % ", data.Humidity);
            RPres = String.Format(" Pressure: {0} hPa ", data.Pressure);
            RTemp = string.Format(" Temperature: {0:f2} °C ", data.Temperature);
        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StatusBar), null);
        }

        private void maxNr_GotFocus(object sender, RoutedEventArgs e)
        {
            MaxServiceNr.Text = "";
        }

        private void NoteGotFocus(object sender, RoutedEventArgs e)
        {
            textBoxNote.Text = "";
        }

        private void parseCheckMaxNR(object sender, RoutedEventArgs e)
        {
            int broj;
            if(!int.TryParse(MaxServiceNr.Text,out broj))
            {
                MaxServiceNr.Text = string.Empty;
                MaxServiceNr.Focus(FocusState.Keyboard);
            }
            else if (broj > 9999 || broj <= 0)
            {
                MaxServiceNr.Text = string.Empty;
                MaxServiceNr.Focus(FocusState.Keyboard);
            }
            else
            {
                MaxServiceNr.Text = broj.ToString();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MaxServiceNr.Text, out inumber))
            {
                app.MaxNrBfrMaintenance = inumber;
            }
            else
            {
                app.MaxNrBfrMaintenance = 1;
            }
            string Datum = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string maxtemp = " Maximal Temperature: " + string.Format("{0:f2} °C", app.MaxTemp);
            string mintemp = " Minimal Temperature: " + string.Format("{0:f2} °C", app.MinTemp);
            string maxhumi = " Maximal Humidity: " + string.Format("{0:f2} %", app.MaxHumi);
            string minhumi = " Minimal Humidity: " + string.Format("{0:f2} %", app.MinHumi);
            string maxpres = " Maximal Pressure: " + string.Format("{0:f2} hPa", app.MaxPres);
            string minpres = " Minimal Pressure: " + string.Format("{0:f2} hPa", app.MinPres);

            if (textBoxNote.Text=="" || textBoxNote.Text == null || textBoxNote.Text == "Enter your note here.") { textBoxNote.Text = " No Entry on Note. Automatic Insert. Maximum calls changed."; }
            app.ReportForMain.Insert(0, new ReportModel { DTofServiceCall = Datum, SCHumidity = RHumi, SCPressure = RPres, SCTemperature = RTemp, MaxNr = "  Maintenance After " + app.currentNrofServiceCalls.ToString() + " Run(s) ", MaxTemperature = maxtemp, MinTemperature = mintemp, MaxPressure = maxpres, MinPressure = minpres, MaxHumidity = maxhumi, MinHumidity = minhumi, Note =" Note: " + textBoxNote.Text });
            app.currentNrofServiceCalls = 0;
            this.Frame.Navigate(typeof(StatusBar), null);
        }

        private void onLoadMSN(object sender, RoutedEventArgs e)
        {
            MaxServiceNr.Text = app.MaxNrBfrMaintenance.ToString();
        }
    }
}
