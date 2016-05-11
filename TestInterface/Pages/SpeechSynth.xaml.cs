﻿using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.SpeechSynthesis;
using Windows.ApplicationModel.Resources.Core;

namespace TestInterface
{
    public partial class SpeechSynth : Page
    {
        private SpeechSynthesizer synthesizer;

        private ResourceContext speechContext;
        private ResourceMap speechResourceMap;

        public SpeechSynth()
        {
            this.InitializeComponent();

            synthesizer = new SpeechSynthesizer();

            keyboard.RegisterTarget(textBoxRead);

            speechContext = ResourceContext.GetForCurrentView();
            speechContext.Languages = new string[] { SpeechSynthesizer.DefaultVoice.Language };

            speechResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("LocalizationTTSResources");

            InitializeListboxVoiceChooser();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdditionalApps), null);
        }

        private void btnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxRead.Text = "";
        }

        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            MediaElement mediaElement = new MediaElement();

            SpeechSynthesisStream stream = await synthesizer.SynthesizeTextToStreamAsync(textBoxRead.Text);

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }

        private void InitializeListboxVoiceChooser()
        {
            // Get all of the installed voices.
            var voices = SpeechSynthesizer.AllVoices;

            // Get the currently selected voice.
            VoiceInformation currentVoice = synthesizer.Voice;

            foreach (VoiceInformation voice in voices.OrderBy(p => p.Language))
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = voice.DisplayName;
                item.Tag = voice;
                item.Content = voice.DisplayName + " (Language: " + voice.Language + ")";
                listBox.Items.Add(item);

                // Check to see if we're looking at the current voice and set it as selected in the listbox.
                if (currentVoice.Id == voice.Id)
                {
                    item.IsSelected = true;
                    listBox.SelectedItem = item;
                }
            }
        }

        private async void onSelectChange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)(listBox.SelectedItem);
            VoiceInformation voice = (VoiceInformation)(item.Tag);
            synthesizer.Voice = voice;

            MediaElement mediaElement = new MediaElement();

            SpeechSynthesisStream stream = await synthesizer.SynthesizeTextToStreamAsync("Hello!");

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
}
