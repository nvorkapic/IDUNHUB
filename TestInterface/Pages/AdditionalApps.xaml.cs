﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdditionalApps : Page
    {
        public AdditionalApps()
        {
            this.InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private void btnSpeech_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SpeechSynth), null);
        }

        private void btnLED_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ALEDC), null);
        }

        private void btnMagnet_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(IMURead), null);
        }
    }
}
