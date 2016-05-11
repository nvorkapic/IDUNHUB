using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TestInterface
{
    public sealed partial class StatusBar : Page
    {
        private TestInterface.App app = Application.Current as TestInterface.App;

        public StatusBar()
        {
            this.InitializeComponent();
        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private void btnSetNrOfRuns_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(statusBarSetting), null);
        }

        private void onLoadCatchCurrUsage(object sender, RoutedEventArgs e)
        {
            btnCurrUsage.Content = "Current\nUsage\nCalls:\n" + app.currentNrofServiceCalls + " / " + app.MaxNrBfrMaintenance;
        }

        private void onLoadList(object sender, RoutedEventArgs e)
        {
            ListViewTest.ItemsSource = app.ReportForMain;
        }
    }
}
