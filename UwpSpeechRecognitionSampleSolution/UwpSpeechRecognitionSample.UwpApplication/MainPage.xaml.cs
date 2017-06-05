using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UwpSpeechRecognition.UserControlLibrary;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpSpeechRecognitionSample.UwpApplication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            this.SpeechRecognitionControl.PhraseRecognisedEvent += SpeechRecognitionControl_PhraseRecognisedEvent;

            this.SpeechRecognitionControl.PassiveListeningStartedEvent += SpeechRecognitionControl_PassiveListeningStartedEventAsync;
            this.SpeechRecognitionControl.PassiveListeningStoppedEvent += SpeechRecognitionControl_PassiveListeningStoppedEvent;

            this.SpeechRecognitionControl.ActiveListeningStartedEvent += SpeechRecognitionControl_ActiveListeningStartedEvent;
            this.SpeechRecognitionControl.ActiveListeningStoppedEvent += SpeechRecognitionControl_ActiveListeningStoppedEvent;
        }

        private void SpeechRecognitionControl_ActiveListeningStoppedEvent(object sender, EventArgs e)
        {
        }

        private void SpeechRecognitionControl_ActiveListeningStartedEvent(object sender, EventArgs e)
        {
            
        }

        private void SpeechRecognitionControl_PassiveListeningStoppedEvent(object sender, EventArgs e)
        {
        }

        private async void SpeechRecognitionControl_PassiveListeningStartedEventAsync(object sender, EventArgs e)
        {
            var dialog = new MessageDialog("Passive Listening Started");
            await dialog.ShowAsync();
        }

        private void SpeechRecognitionControl_PhraseRecognisedEvent(object sender, EventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.SpeechRecognitionControl.Initialise();
        }
    }
}
