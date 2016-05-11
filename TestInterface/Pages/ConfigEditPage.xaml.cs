using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestInterface.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestInterface.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigEditPage : Page
    {
        public ConfigModel Config { get; set; }

        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        string value;
        double number;

        public ConfigEditPage()
        {
            this.InitializeComponent();

            EnableUsageThreshold.Checked += EnableUsageThreshold_Change;
            EnableUsageThreshold.Unchecked += EnableUsageThreshold_Change;

            EnableConfig.Checked += EnableConfig_Change;
            EnableConfig.Unchecked += EnableConfig_Change;



        }
      

        private void EnableConfig_Change(object sender, RoutedEventArgs e)
        {
            DefBtn.IsEnabled = !DefBtn.IsEnabled;

            if (EnableConfig.IsChecked == true)
            {
                (Application.Current as TestInterface.App).DisplayOptionsList.Where(x => x.Title == Config.Measurement).First().Enabled = " ►";
                ConfigMainPage.Current.RefreshList((Application.Current as TestInterface.App).DisplayOptionsList.ToList());


            }
            else
            {
                (Application.Current as TestInterface.App).DisplayOptionsList.Where(x => x.Title == Config.Measurement).First().Enabled = "";
                ConfigMainPage.Current.RefreshList((Application.Current as TestInterface.App).DisplayOptionsList.ToList());
            }

            if (ConfigurationPanel.Visibility == Visibility.Collapsed)
            {
                ConfigurationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ConfigurationPanel.Visibility = Visibility.Collapsed;
            }

        }

        private void EnableUsageThreshold_Change(object sender, RoutedEventArgs e)
        {
            
            ThresholdBottom.IsEnabled = !ThresholdBottom.IsEnabled;
            ThresholdCeiling.IsEnabled = !ThresholdCeiling.IsEnabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            Config = e.Parameter as ConfigModel;
            this.DataContext = Config;

        }

        private void LoadDefaults(object sender, RoutedEventArgs e)
        {
            Config.Report = 1;
            Config.Interval = 1000;
            Config.Threshold = false;
            Config.ThresholdBottomValue = null;
            Config.ThresholdCeilingValue = null;

            IntervalSetting.Text = "1000";
            EnableUsageThreshold.IsChecked = Config.Threshold;
            ThresholdBottom.Text = "";
            ThresholdBottom.IsEnabled = false;
            ThresholdCeiling.Text = "";
            ThresholdCeiling.IsEnabled = false;
            FirstOptionReporting.IsChecked = true;

        }

        private void SaveDataBoxBot(object sender, RoutedEventArgs e)
        {
            

            value = ThresholdBottom.Text;

            if (double.TryParse(value, out number))
            {
                Config.ThresholdBottomValue = number;
            }
            else
            {
                ThresholdBottom.Text="0";
            }
            

        }
        private void SaveDataBoxCei(object sender, RoutedEventArgs e)
        {

            value = ThresholdCeiling.Text;

            if (double.TryParse(value, out number))
            {
                Config.ThresholdCeilingValue = number;
            }
           else
            {
                ThresholdCeiling.Text="0";
            }

        }

        private void PanelOnLoad(object sender, RoutedEventArgs e)
        {
            if (Config.Enabled == true)
            {
                ConfigurationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ConfigurationPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void PageOnLoad(object sender, RoutedEventArgs e)
        {
            if (Config.Measurement == "Usage")
            {
                Image img = new Image();
                BitmapImage bitmap = new BitmapImage(new Uri("ms-appx://MainApp/Assets/Finger.png"));


                ImageTitle.Source = bitmap;
            }
            if (Config.Measurement == "Temperature")
            {
                Image img = new Image();
                BitmapImage bitmap = new BitmapImage( new Uri("ms-appx://MainApp/Assets/whitethermometer.png"));
                

                ImageTitle.Source = bitmap;
                
            }
            if (Config.Measurement == "Pressure")
            {
                Image img = new Image();
                BitmapImage bitmap = new BitmapImage(new Uri("ms-appx://MainApp/Assets/pressurex.png"));


                ImageTitle.Source = bitmap;

            }
            if (Config.Measurement == "Humidity")
            {
                Image img = new Image();
                BitmapImage bitmap = new BitmapImage(new Uri("ms-appx://MainApp/Assets/humidity.png"));


                ImageTitle.Source = bitmap;

            }
            if (Config.Measurement == "Accelerometer")
            {
                Image img = new Image();
                BitmapImage bitmap = new BitmapImage(new Uri("ms-appx://MainApp/Assets/speedometer.png"));

                ImageTitle.Source = bitmap;

            }
            if (Config.Measurement == "Magnetometer")
            {
                Image img = new Image();
                BitmapImage bitmap = new BitmapImage(new Uri("ms-appx://MainApp/Assets/magnet.png"));

                ImageTitle.Source = bitmap;

            }
            if (Config.Measurement == "Gyroscope")
            {
                Image img = new Image();
                BitmapImage bitmap = new BitmapImage(new Uri("ms-appx://MainApp/Assets/gyro.png"));

                ImageTitle.Source = bitmap;

            }
        }
    }

    //public class EnableToColorConverter : IValueConverter
    //{

    //    public object Convert(object value, Type targetType, object parameter, string language)
    //    {
    //        if ((parameter as ConfigModel).Enabled) { return new SolidColorBrush(Colors.Green); } else { return new SolidColorBrush(Colors.Red); }
             
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, string language)
    //    {
    //        throw new Exception("Not Implemented");
    //    }
    //}
}
