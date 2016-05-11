using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestInterface
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }

    public class HubViewModel : BaseViewModel
    {

        private Page selectedPage;
        public Page SelectedPage
        {
            get { return selectedPage; }
            set { selectedPage = value; Notify(); }
        }
    }

 

    public sealed partial class HubInterface : Page
    {
        private HubViewModel ViewModel;

        public HubInterface()
        {
            this.InitializeComponent();

            ViewModel = new HubViewModel();
            this.DataContext = ViewModel;
        }

        private void LoadListView(object sender, RoutedEventArgs e)
        {
            var listView = (ListView)sender;
            
            listView.ItemsSource = (Application.Current as App).MeasurementConfiguration;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Config = (sender as ListView).SelectedItem as ConfigModel;
            HubHeaderText.Text = Config.Measurement;
            
            ViewModel.SelectedPage = Activator.CreateInstance(Config.PageType) as Page;
            

        }
    }

    


}
