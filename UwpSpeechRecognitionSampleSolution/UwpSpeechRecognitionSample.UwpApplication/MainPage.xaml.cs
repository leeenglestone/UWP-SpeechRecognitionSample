using System;
using System.Linq;
using System.Threading.Tasks;
using UwpSpeechRecognition.UserControlLibrary.EventArgs;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UwpSpeechRecognitionSample.UwpApplication
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.SpeechRecognitionControl.PhraseRecognisedEvent += SpeechRecognitionControl_PhraseRecognisedEventAsync;

            this.SpeechRecognitionControl.PassiveListeningStartedEvent += SpeechRecognitionControl_PassiveListeningStartedEventAsync;
            this.SpeechRecognitionControl.PassiveListeningStoppedEvent += SpeechRecognitionControl_PassiveListeningStoppedEvent;

            this.SpeechRecognitionControl.ActiveListeningStartedEvent += SpeechRecognitionControl_ActiveListeningStartedEventAsync;
            this.SpeechRecognitionControl.ActiveListeningStoppedEvent += SpeechRecognitionControl_ActiveListeningStoppedEvent;
        }

        private void SpeechRecognitionControl_ActiveListeningStoppedEvent(object sender, EventArgs e)
        {
        }

        private async void SpeechRecognitionControl_ActiveListeningStartedEventAsync(object sender, EventArgs e)
        {
            //var dialog = new MessageDialog("Active Listening Started");
            //await dialog.ShowAsync();
        }

        private void SpeechRecognitionControl_PassiveListeningStoppedEvent(object sender, EventArgs e)
        {
        }

        private async void SpeechRecognitionControl_PassiveListeningStartedEventAsync(object sender, EventArgs e)
        {
            //var dialog = new MessageDialog("Passive Listening Started");
            //await dialog.ShowAsync();
        }

        private async void SpeechRecognitionControl_PhraseRecognisedEventAsync(object sender, PhraseRecognisedEventArgs e)
        {

            if (new[] { "what's your name" }.Contains(e.RecognisedPhrase))
            {
                // Put a bit of a wait here
                Task.Delay(TimeSpan.FromSeconds(2)).Wait();

                Speak($"You can call me {SpeechRecognitionControl.ViewModel.AwakePhrase}");
            }
            else if (new[] { "what's the date", "what's todays date", "what's the date today" }.Contains(e.RecognisedPhrase))
            {
                //Speak("How should I know?");

                Speak("Today is the 7th June");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.SpeechRecognitionControl.Initialise();
            this.DataContext = this.SpeechRecognitionControl.ViewModel;
        }

        private async void Speak(string phrase)
        {
            MediaElement mediaElement = this.mediaElement;
            var _speechSynthesizer = new SpeechSynthesizer();
            SpeechSynthesisStream stream = await _speechSynthesizer.SynthesizeTextToStreamAsync(phrase);
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
}
