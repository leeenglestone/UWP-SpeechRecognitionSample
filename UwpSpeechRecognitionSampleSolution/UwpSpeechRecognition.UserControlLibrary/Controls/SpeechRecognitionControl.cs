using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpSpeechRecognition.UserControlLibrary.EventArgs;
using UwpSpeechRecognition.UserControlLibrary.Models;
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

        private SpeechRecognitionViewModel _viewModel = new SpeechRecognitionViewModel();
        public SpeechRecognitionViewModel ViewModel
        {
            get
            {
                if (this.DataContext == null)
                    this.DataContext = _viewModel;

                return (this.DataContext as SpeechRecognitionViewModel);
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await (this.DataContext as SpeechRecognitionViewModel).Initialize();

            ViewModel.CommandPhraseRecognised += RecognisedPhraseEvent;

            // Hookup view model events for stop/starting listening
            ViewModel.ActiveListeningStartedEvent += ViewModel_ActiveListeningStartedEvent;
            ViewModel.ActiveListeningStoppedEvent += ViewModel_ActiveListeningStoppedEvent;
            ViewModel.PassiveListeningStartedEvent += ViewModel_PassiveListeningStartedEvent;
            ViewModel.PassiveListeningStoppedEvent += ViewModel_PassiveListeningStoppedEvent;
        }

        private void ViewModel_PassiveListeningStoppedEvent(object sender, System.EventArgs e)
        {
            PassiveListeningStoppedEvent(this, e);
        }

        private void ViewModel_PassiveListeningStartedEvent(object sender, System.EventArgs e)
        {
            PassiveListeningStartedEvent(this, e);
        }

        private void ViewModel_ActiveListeningStoppedEvent(object sender, System.EventArgs e)
        {
            ActiveListeningStoppedEvent(this, e);
        }

        private void ViewModel_ActiveListeningStartedEvent(object sender, System.EventArgs e)
        {
            ActiveListeningStartedEvent(this, e);
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
