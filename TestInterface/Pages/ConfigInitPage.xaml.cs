using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using static TestInterface.App;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestInterface.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigInitPage : Page
    {
        public bool Pass;

        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public ConfigInitPage()
        {
            this.InitializeComponent();
        }


        private async void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ObjectIDTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(ObjectNameTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(ObjectDescriptionTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(MaintenanceOrgIDTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(MaintenanceOrgNameTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(SiteTB.Text)) { Pass = false; } else { Pass = true; }

            if (Pass == true)
            {
                List<InitModel> InitList = new List<InitModel>();

                InitList.Add(new InitModel { Name = "ObjectID", Value = ObjectIDTB.Text });
                InitList.Add(new InitModel { Name = "ObjectName", Value = ObjectNameTB.Text });
                InitList.Add(new InitModel { Name = "ObjectDescription", Value = ObjectDescriptionTB.Text });
                InitList.Add(new InitModel { Name = "MaintenanceOrgID", Value = MaintenanceOrgIDTB.Text });
                InitList.Add(new InitModel { Name = "MaintenanceOrgName", Value = MaintenanceOrgNameTB.Text });
                InitList.Add(new InitModel { Name = "Site", Value = SiteTB.Text });

                string json = JsonConvert.SerializeObject(InitList.ToArray(), Formatting.Indented);
                StorageFile InitFile = await localFolder.CreateFileAsync("InitializationJSON.txt", CreationCollisionOption.ReplaceExisting);

                await FileIO.WriteTextAsync(InitFile, json);
   
            }

        }

        private void ContinueCLick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ObjectIDTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(ObjectNameTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(ObjectDescriptionTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(MaintenanceOrgIDTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(MaintenanceOrgNameTB.Text)) { Pass = false; } else { Pass = true; }
            if (string.IsNullOrWhiteSpace(SiteTB.Text)) { Pass = false; } else { Pass = true; }

            if (Pass == true)
            {
                var x = (Application.Current as TestInterface.App);

                x.DisplayOptionsList.Remove(x.DisplayOptionsList.Single(y => y.Title == "Initialization"));

                x.DisplayOptionsList.Add(new DisplayOption { Title = "Usage", ClassType = typeof(Pages.ConfigEditPage), Enabled = "" });
                x.DisplayOptionsList.Add(new DisplayOption { Title = "Temperature", ClassType = typeof(Pages.ConfigEditPage), Enabled = "" });
                x.DisplayOptionsList.Add(new DisplayOption { Title = "Pressure", ClassType = typeof(Pages.ConfigEditPage), Enabled = "" });
                x.DisplayOptionsList.Add(new DisplayOption { Title = "Humidity", ClassType = typeof(Pages.ConfigEditPage), Enabled = "" });
                x.DisplayOptionsList.Add(new DisplayOption { Title = "Accelerometer", ClassType = typeof(Pages.ConfigEditPage), Enabled = "" });
                x.DisplayOptionsList.Add(new DisplayOption { Title = "Magnetometer", ClassType = typeof(Pages.ConfigEditPage), Enabled = "" });
                x.DisplayOptionsList.Add(new DisplayOption { Title = "Gyroscope", ClassType = typeof(Pages.ConfigEditPage), Enabled = "" });
                x.DisplayOptionsList.Add(new DisplayOption { Title = "Finalize", ClassType = typeof(Pages.ConfigFinalizePage) });

                ConfigMainPage.Current.IndexFocus();
            }
        }

        private async void ShowInitSetting(object sender, RoutedEventArgs e)
        {

            try
            {
                StorageFile InitFile = await localFolder.GetFileAsync("InitializationJSON.txt");
                string InitiText = await FileIO.ReadTextAsync(InitFile);

                InitText.Text = InitiText;

            }
            catch
            {
                InitText.Text = "No data found!";
            }
        }

        private async void LoadPreviousSet(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFile InitFile = await localFolder.GetFileAsync("InitializationJSON.txt");
                string InitiText = await FileIO.ReadTextAsync(InitFile);

                List<InitModel> InitList = JsonConvert.DeserializeObject<List<InitModel>>(InitiText);

                ObjectIDTB.Text = InitList.Where(x => x.Name == "ObjectID").First().Value;
                ObjectNameTB.Text = InitList.Where(x => x.Name == "ObjectName").First().Value;
                ObjectDescriptionTB.Text = InitList.Where(x => x.Name == "ObjectDescription").First().Value;
                MaintenanceOrgIDTB.Text = InitList.Where(x => x.Name == "MaintenanceOrgID").First().Value;
                MaintenanceOrgNameTB.Text = InitList.Where(x => x.Name == "MaintenanceOrgName").First().Value;
                SiteTB.Text = InitList.Where(x => x.Name == "Site").First().Value;

            }
            catch
            {
                ObjectIDTB.Text = string.Empty;
                ObjectNameTB.Text = string.Empty;
                ObjectDescriptionTB.Text = string.Empty;
                MaintenanceOrgIDTB.Text = string.Empty;
                MaintenanceOrgNameTB.Text = string.Empty;
                SiteTB.Text = string.Empty;

                InitText.Text = "No data found!";
            }
        }
    }
}
