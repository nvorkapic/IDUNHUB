using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestInterface.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static TestInterface.App;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestInterface.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigMainPage : Page
    {
        
        public static ConfigMainPage Current { get; set; }

        public ConfigMainPage()
        {
            this.InitializeComponent();


            if (!(Application.Current as TestInterface.App).doInit)
            {
                (Application.Current as TestInterface.App).DisplayOptionsList.Add(new DisplayOption { Title = "Initialization", ClassType = typeof(ConfigInitPage) });
            }

            (Application.Current as TestInterface.App).DisplayOptionsList.CollectionChanged += DisplayOptionsList_CollectionChanged;
        }

        private void DisplayOptionsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DisplayListBox.ItemsSource = (Application.Current as TestInterface.App).DisplayOptionsList;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ConfigMainPage.Current = this;

            DisplayListBox.ItemsSource = (Application.Current as TestInterface.App).DisplayOptionsList;
            if (Window.Current.Bounds.Width < 640)
            {
                DisplayListBox.SelectedIndex = -1;

            }
            else
            {
                DisplayListBox.SelectedIndex = 0;
            }
        }

        private void DisplayControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListBox DisplayListBox = sender as ListBox;
            DisplayOption s = DisplayListBox.SelectedItem as DisplayOption;
            if (s != null)
            {
                var Config = (Application.Current as App).MeasurementConfiguration.Where(x => x.Measurement == s.Title).SingleOrDefault();
                DisplayFrame.Navigate(s.ClassType, Config);
            }
        }


        public void RefreshList(List<DisplayOption> NewList)
        {
            DisplayListBox.ItemsSource = NewList;
        }

        public void IndexFocus()
        {
            DisplayListBox.SelectedIndex = 0;
        }

        public void NavigateToMain()
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }
        //public class DisplayBindingConverter : IValueConverter
        //{

        //    public object Convert(object value, Type targetType, object parameter, string language)
        //    {
        //        DisplayOption s = value as DisplayOption;
        //        return s.Title;

        //    }

        //    public object ConvertBack(object value, Type targetType, object parameter, string language)
        //    {
        //        return true;
        //    }
        //}

    }
}
