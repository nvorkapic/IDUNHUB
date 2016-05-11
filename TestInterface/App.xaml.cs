using SenseHat;
using System;
using System.Collections.ObjectModel;
using TestInterface.Models;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TestInterface
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public enum Device
        {
            Phone,
            Tablet,
            Desktop,
            Xbox,
            Iot,
            Continuum
        };
        public Device DeviceType;

        public int MaxNrBfrMaintenance;
        public int currentNrofServiceCalls;

        public float MaxTemp;
        public float MinTemp;

        public float MaxHumi;
        public float MinHumi;

        public float MaxPres;
        public float MinPres;

        public ObservableCollection<ReportModel> ReportForMain = new ObservableCollection<ReportModel>();

        //public SensorReader SensorReader { get; private set; }

        public ObservableCollection<DisplayOption> DisplayOptionsList = new ObservableCollection<DisplayOption>();
        public ObservableCollection<ConfigModel> MeasurementConfiguration = new ObservableCollection<ConfigModel>();



        public bool doInit;
        public bool doConfig;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);

            AppData.InitCloud();
            AppData.InitSensor();

            this.InitializeComponent();
            this.Suspending += OnSuspending;
//#if ARM
//            SensorReader = new SenseHatReader(1000, 20);
//#else
//            SensorReader = new DummyReader(1000, 20);
//#endif
//            SensorReader.Init();

            MeasurementConfiguration.Add(new ConfigModel { Measurement = "Usage", Report = 1, Interval = 1000 });
            MeasurementConfiguration.Add(new ConfigModel { Measurement = "Temperature", Report = 1, Interval = 1000 });
            MeasurementConfiguration.Add(new ConfigModel { Measurement = "Pressure", Report = 1, Interval = 1000 });
            MeasurementConfiguration.Add(new ConfigModel { Measurement = "Humidity", Report = 1, Interval = 1000 });
            MeasurementConfiguration.Add(new ConfigModel { Measurement = "Magnetometer", Report = 1, Interval = 1000 });
            MeasurementConfiguration.Add(new ConfigModel { Measurement = "Accelerometer", Report = 1, Interval = 1000 });
            MeasurementConfiguration.Add(new ConfigModel { Measurement = "Gyroscope", Report = 1, Interval = 1000 });


            MaxNrBfrMaintenance = 1;
            currentNrofServiceCalls = 0;

            MaxTemp = float.NegativeInfinity;
            MinTemp = float.PositiveInfinity;
            MaxHumi = float.NegativeInfinity;
            MinHumi = float.PositiveInfinity;
            MaxPres = float.NegativeInfinity;
            MinPres = float.PositiveInfinity;

            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.IoT")
            {
                DeviceType = Device.Iot;
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            //#if ARM
            //            Frame rootFrame = Window.Current.Content as Frame;

            //            if (rootFrame == null)
            //            {
            //                 Create a Frame to act as the navigation context and navigate to the first page
            //                rootFrame = new Frame();

            //                rootFrame.NavigationFailed += OnNavigationFailed;

            //                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            //                {
            //                    TODO: Load state from previously suspended application
            //                }

            //                 Place the frame in the current Window
            //                Window.Current.Content = rootFrame;
            //            }

            //               if (e.PrelaunchActivated == false)
            //            {
            //                if (rootFrame.Content == null)
            //                {
            //                     When the navigation stack isn't restored navigate to the first page,
            //                     configuring the new page by passing required information as a navigation
            //                     parameter
            //                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
            //                }
            //                 Ensure the current window is active
            //                Window.Current.Activate();
            //            }

            //#else 
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
            //#endif

        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

    }
}
