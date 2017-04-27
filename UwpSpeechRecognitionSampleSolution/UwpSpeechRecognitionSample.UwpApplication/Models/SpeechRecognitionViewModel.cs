using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UwpSpeechRecognitionSample.UwpApplication.Enums;
using Windows.Media.Capture;
using Windows.Media.SpeechRecognition;
using Windows.UI.Xaml;

namespace UwpSpeechRecognitionSample.UwpApplication.Models
{
    public class SpeechRecognitionViewModel : INotifyPropertyChanged
    {
        SpeechRecognizer _awakeSpeechRecognizer;
        SpeechRecognizer _commandSpeechRecognizer = new SpeechRecognizer();

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

                switch (_listeningState)
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

        public async Task Initialize()
        {
            if (!(await CheckForMicrophonePermission()))
                return;

            _awakeSpeechRecognizer = new SpeechRecognizer();
            _awakeSpeechRecognizer.Constraints.Add(new SpeechRecognitionListConstraint(new List<String>() { "Awake Commands" }, "start"));
            var result = await _awakeSpeechRecognizer.CompileConstraintsAsync();

            if (result.Status != SpeechRecognitionResultStatus.Success)
            {
                return;
            }

            _awakeSpeechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
            await _awakeSpeechRecognizer.ContinuousRecognitionSession.StartAsync(SpeechContinuousRecognitionMode.Default);

            //_speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
            //await _speechRecognizer.ContinuousRecognitionSession.StartAsync(SpeechContinuousRecognitionMode.Default);
        }

        private async Task<bool> CheckForMicrophonePermission()
        {
            try
            {
                // Request access to the microphone 
                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
                settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
                settings.MediaCategory = MediaCategory.Speech;
                MediaCapture capture = new MediaCapture();

                await capture.InitializeAsync(settings);
            }
            catch (UnauthorizedAccessException)
            {
                // The user has turned off access to the microphone. If this occurs, we should show an error, or disable
                // functionality within the app to ensure that further exceptions aren't generated when 
                // recognition is attempted.
                return false;
            }

            return true;
        }

        private void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            if (args.Result.Confidence == SpeechRecognitionConfidence.High || args.Result.Confidence == SpeechRecognitionConfidence.Medium)
            {
                //Helpers.RunOnCoreDispatcherIfPossible(() => WakeUpAndListen(), false
                Helpers.Helpers.RunOnCoreDispatcherIfPossible(() => WakeUpAndListen(), false);
            }
        }

        private async Task WakeUpAndListen()
        {
            await _awakeSpeechRecognizer.ContinuousRecognitionSession.CancelAsync();

            ListeningState = ListeningState.AnalysingSpeech;

            // Do some stuff

            await _awakeSpeechRecognizer.ContinuousRecognitionSession.StartAsync();

            ListeningState = ListeningState.PassiveListening;
        }

        private void _speechRecognizer_HypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs args)
        {
            Helpers.Helpers.RunOnCoreDispatcherIfPossible(() => RecognisedPhrase = args.Hypothesis.Text.ToLower(), false);
        }
    }
}
