using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpSpeechRecognition.UserControlLibrary.EventArgs;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UwpSpeechRecognition.UserControlLibrary.Controls
{
    public class SpeechRecognitionControl : UserControl
    {
        public event EventHandler ActiveListeningStartedEvent;
        public event EventHandler ActiveListeningStoppedEvent;

        public event EventHandler PassiveListeningStartedEvent;
        public event EventHandler PassiveListeningStoppedEvent;

        public event EventHandler<PhraseRecognisedEventArgs> PhraseRecognisedEvent;

        public SpeechRecognitionViewModel ViewModel
        {
            get
            {
                return (this.DataContext as SpeechRecognitionViewModel);
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await (this.DataContext as SpeechRecognitionViewModel).Initialize();

            ViewModel.CommandPhraseRecognised += RecognisedPhraseEvent;
            
            // Hookup view model events for stop/starting listening
        }

        public void RecognisedPhraseEvent(object sender, PhraseRecognisedEventArgs args)
        {
            ViewModel.RecognisedPhrase += args.RecognisedPhrase;

            PhraseRecognisedEvent(this, args);

        }

        public void Initialise()
        {
            ViewModel.Initialize();
        }

        public void AddPhrases(string[] phrases)
        {
            ViewModel.AddPhrases(phrases);
        }
    }
}
