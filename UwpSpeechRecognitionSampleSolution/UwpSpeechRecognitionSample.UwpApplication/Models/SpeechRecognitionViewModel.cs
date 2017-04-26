using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UwpSpeechRecognitionSample.UwpApplication.Enums;
using Windows.UI.Xaml;

namespace UwpSpeechRecognitionSample.UwpApplication.Models
{
    public class SpeechRecognitionViewModel : INotifyPropertyChanged
    {
        private ListeningState _listeningState = ListeningState.PassiveListening;
        public ListeningState ListeningState
        {
            get
            {
                return _listeningState;
            }
            set
            {
                _listeningState = value;
                
                switch(_listeningState)
                {
                    case ListeningState.PassiveListening:
                        StopListeningButtonVisibility = Visibility.Visible;
                        StartListeningButtonVisibility = Visibility.Collapsed;
                        break;

                    case ListeningState.NotListening:
                        StopListeningButtonVisibility = Visibility.Collapsed;
                        StartListeningButtonVisibility = Visibility.Visible;
                        break;
                }

                NotifyPropertyChanged();
            }
        }

        private string _listeningStateText = string.Empty;
        public string ListeningStateText { get { return _listeningStateText; } set { _listeningStateText = value; NotifyPropertyChanged(); } }

        private string _actionResult = string.Empty;
        public string ActionResult { get { return _actionResult; } set { _actionResult = value; NotifyPropertyChanged(); } }

        private string _recognisedPhrase = string.Empty;
        public string RecognisedPhrase { get { return _recognisedPhrase; } set { _recognisedPhrase = value; NotifyPropertyChanged(); } }

        private Visibility _stopListeningButtonVisibility = Visibility.Visible;
        public Visibility StopListeningButtonVisibility { get { return _stopListeningButtonVisibility; } set { _stopListeningButtonVisibility = value; NotifyPropertyChanged(); } }

        private Visibility _startListeningButtonVisibility = Visibility.Collapsed;
        public Visibility StartListeningButtonVisibility { get { return _startListeningButtonVisibility; } set { _startListeningButtonVisibility = value; NotifyPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
