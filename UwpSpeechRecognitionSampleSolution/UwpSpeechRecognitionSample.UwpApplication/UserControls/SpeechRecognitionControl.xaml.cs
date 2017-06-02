using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UwpSpeechRecognitionSample.UwpApplication.EventArgs;
using UwpSpeechRecognitionSample.UwpApplication.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UwpSpeechRecognitionSample.UwpApplication.UserControls
{
    public sealed partial class SpeechRecognitionControl : UserControl
    {

        public SpeechRecognitionViewModel ViewModel
        {
            get
            {
               return  (this.DataContext as SpeechRecognitionViewModel);
            }
        }

        public SpeechRecognitionControl()
        {
            this.InitializeComponent();
        }

        private void BtnStopListening_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ListeningState = Enums.ListeningState.NotListening;
        }

        private void BtnStartListening_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ListeningState = Enums.ListeningState.PassiveListening;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await (this.DataContext as SpeechRecognitionViewModel).Initialize();

            ViewModel.CommandPhraseRecognised += RecognisedPhraseEvent;
        }

        public void RecognisedPhraseEvent(object sender, PhraseRecognisedEventArgs  args)
        {
            ViewModel.RecognisedPhrase = args.RecognisedPhrase;
        }
    }
}
