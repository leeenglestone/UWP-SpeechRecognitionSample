using System;
using UwpSpeechRecognition.UserControlLibrary.EventArgs;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            if (e.RecognisedPhrase == "what time is it")
            {
                var dialog = new MessageDialog("The time is {DateTime.Now.ToString()");
                await dialog.ShowAsync();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.SpeechRecognitionControl.Initialise();

            this.DataContext = this.SpeechRecognitionControl.ViewModel;

            Speak();

        }

        private async void Speak()
        {
            MediaElement mediaElement = this.mediaElement;
            var _speechSynthesizer = new SpeechSynthesizer();
            SpeechSynthesisStream stream = await _speechSynthesizer.SynthesizeTextToStreamAsync("Initialising speech synthesis");
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
}
