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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestInterface.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigFinalizePage : Page
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public ConfigFinalizePage()
        {
            this.InitializeComponent();

            (Application.Current as App).DisplayOptionsList.CollectionChanged += DisplayOptionsList_CollectionChanged;
        }

        private void DisplayOptionsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ConfigMainPage.Current.RefreshList((Application.Current as TestInterface.App).DisplayOptionsList.ToList());
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            

            string json = JsonConvert.SerializeObject((Application.Current as App).MeasurementConfiguration.ToArray(), Formatting.Indented);
            StorageFile ConfigFile = await localFolder.CreateFileAsync("ConfigurationJSON.txt", CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(ConfigFile,json);
        }

        private async void ShowConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFile ConfigFile = await localFolder.GetFileAsync("ConfigurationJSON.txt");
                string ConfigText = await FileIO.ReadTextAsync(ConfigFile);

                ShowConfigTB.Text = ConfigText;
            }
            catch
            {
                ShowConfigTB.Text = "No data found!";
            }
            
        }

        private void LoadSaved(object sender, RoutedEventArgs e)
        {
            ConfigMainPage.Current.RefreshList((Application.Current as TestInterface.App).DisplayOptionsList.ToList());
            LoadData();
      
        }


        public async void LoadData()
        {
            StorageFile ConfigFile = await localFolder.GetFileAsync("ConfigurationJSON.txt");
            string ConfigText = await FileIO.ReadTextAsync(ConfigFile);

            ObservableCollection<ConfigModel> InitList = JsonConvert.DeserializeObject<ObservableCollection<ConfigModel>>(ConfigText);

            (Application.Current as App).MeasurementConfiguration = InitList;
        }

        private void ToMain(object sender, RoutedEventArgs e)
        {
            ConfigMainPage.Current.NavigateToMain();
        }
    }
}
