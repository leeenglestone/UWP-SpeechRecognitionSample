using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UwpSpeechRecognition.UserControlLibrary.Enums;
using UwpSpeechRecognition.UserControlLibrary.EventArgs;
using Windows.Media.Capture;
using Windows.Media.SpeechRecognition;
using Windows.UI.Xaml;

namespace UwpSpeechRecognition.UserControlLibrary.Models
{
    public class SpeechRecognitionViewModel : INotifyPropertyChanged
    {
        public EventHandler<PhraseRecognisedEventArgs> CommandPhraseRecognised;
        public event EventHandler ActiveListeningStartedEvent;
        public event EventHandler ActiveListeningStoppedEvent;
        public event EventHandler PassiveListeningStartedEvent;
        public event EventHandler PassiveListeningStoppedEvent;

        SpeechRecognizer _awakeSpeechRecognizer;
        SpeechRecognizer _commandSpeechRecognizer;
        DispatcherTimer _activeListeningTimer;

        private string _awakePhrase = "Oracle";
        public string AwakePhrase { get { return _awakePhrase; } set { _awakePhrase = value; NotifyPropertyChanged(); } }

        public List<string> Phrases { get; set; } = new List<string>();

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
                        StopListeningButtonVisibility = Visibility.Collapsed;
                        StartListeningButtonVisibility = Visibility.Visible;
                        break;

                    case ListeningState.NotListening:
                        StopListeningButtonVisibility = Visibility.Collapsed;
                        StartListeningButtonVisibility = Visibility.Visible;
                        break;

                    case ListeningState.ActiveListening:
                        StopListeningButtonVisibility = Visibility.Visible;
                        StartListeningButtonVisibility = Visibility.Collapsed;
                        break;
                }

                NotifyPropertyChanged();
            }
        }

        internal void AddPhrases(string[] phrases)
        {
            Phrases.AddRange(phrases);
        }

        private string _listeningStateText = string.Empty;
        public string ListeningStateText { get { return _listeningStateText; } set { _listeningStateText = value; NotifyPropertyChanged(); } }

        private string _actionResult = string.Empty;
        public string ActionResult { get { return _actionResult; } set { _actionResult = value; NotifyPropertyChanged(); } }

        private string _recognisedPhrase = string.Empty;
        public string RecognisedPhrase { get { return _recognisedPhrase; } set { _recognisedPhrase = value; NotifyPropertyChanged(); } }

        private Visibility _stopListeningButtonVisibility = Visibility.Collapsed;
        public Visibility StopListeningButtonVisibility { get { return _stopListeningButtonVisibility; } set { _stopListeningButtonVisibility = value; NotifyPropertyChanged(); } }

        private Visibility _startListeningButtonVisibility = Visibility.Visible;
        public Visibility StartListeningButtonVisibility { get { return _startListeningButtonVisibility; } set { _startListeningButtonVisibility = value; NotifyPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Initialize()
        {
            if (!(await CheckForMicrophonePermission()))
                return;

            var setupAwakeSpeechRecogniserResult = await SetupAwakeSpeechRecogniserAsync();
            if (setupAwakeSpeechRecogniserResult.Status != SpeechRecognitionResultStatus.Success)
                return;

            var setupCommandSpeechRecogniserResult = await SetupCommandSpeechRecogniserAsync();
            if (setupCommandSpeechRecogniserResult.Status != SpeechRecognitionResultStatus.Success)
                return;

            SetupActiveListeningTimer();
        }

        private void SetupActiveListeningTimer()
        {
            _activeListeningTimer = new DispatcherTimer();
            _activeListeningTimer.Interval = new TimeSpan(0, 0, 5);
            _activeListeningTimer.Tick += _activeListeningTimer_Tick;
        }

        private async Task<SpeechRecognitionCompilationResult> SetupAwakeSpeechRecogniserAsync()
        {
            _awakeSpeechRecognizer = new SpeechRecognizer();

            _awakeSpeechRecognizer.Constraints.Add(new SpeechRecognitionListConstraint(new List<String>() { AwakePhrase }, "Awake"));
            var result = await _awakeSpeechRecognizer.CompileConstraintsAsync();

            _awakeSpeechRecognizer.ContinuousRecognitionSession.ResultGenerated += AwakeContinuousRecognitionSession_ResultGenerated;
            await _awakeSpeechRecognizer.ContinuousRecognitionSession.StartAsync(SpeechContinuousRecognitionMode.Default);

            ListeningState = ListeningState.PassiveListening;
            PassiveListeningStartedEvent(this, null);

            return result;
        }

        private async Task<SpeechRecognitionCompilationResult> SetupCommandSpeechRecogniserAsync()
        {
            _commandSpeechRecognizer = new SpeechRecognizer();

            // A pre defined list can be provided here
            // var listConstraint = new SpeechRecognitionListConstraint(Phrases, "Phrases");
            // _commandSpeechRecognizer.Constraints.Add(listConstraint);

            var result = await _commandSpeechRecognizer.CompileConstraintsAsync();
            _commandSpeechRecognizer.HypothesisGenerated += _commandSpeechRecognizer_HypothesisGenerated;
            _commandSpeechRecognizer.ContinuousRecognitionSession.ResultGenerated += _commandSpeechRecognizer_ResultGenerated;
            return result;
        }

        private void _commandSpeechRecognizer_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
        }

        private void _commandSpeechRecognizer_HypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs args)
        {
            Helpers.UiHelper.RunOnCoreDispatcherIfPossible(() => RecognisedPhrase = args.Hypothesis.Text.ToLower(), false);

            var eventArgs = new PhraseRecognisedEventArgs();
            eventArgs.RecognisedPhrase = args.Hypothesis.Text;
            Helpers.UiHelper.RunOnCoreDispatcherIfPossible(() => CommandPhraseRecognised(this, eventArgs), false);
        }


        private void AwakeContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            if (args.Result.Confidence == SpeechRecognitionConfidence.High || args.Result.Confidence == SpeechRecognitionConfidence.Medium)
            {
                Helpers.UiHelper.RunOnCoreDispatcherIfPossible(() => WakeUpAndListen(), false);
            }
        }

        private async Task WakeUpAndListen()
        {
            // Stop passive listeing for awake command
            await _awakeSpeechRecognizer.ContinuousRecognitionSession.CancelAsync();
            PassiveListeningStoppedEvent(this, null);

            // Start active listening for command phrase
            await _commandSpeechRecognizer.ContinuousRecognitionSession.StartAsync();
            ActiveListeningStartedEvent(this, null);
            ListeningState = ListeningState.ActiveListening;

            // Only wait for command for a certain period, then go back to passive listening
            _activeListeningTimer.Start();
        }

        private void _activeListeningTimer_Tick(object sender, object e)
        {
            _activeListeningTimer.Stop();

            ListeningState = ListeningState.PassiveListening;

            try
            {
                ActiveListeningStoppedEvent(this, null);

                Helpers.UiHelper.RunOnCoreDispatcherIfPossible(() => _commandSpeechRecognizer.ContinuousRecognitionSession.CancelAsync(), false);
            }
            catch { }

            Task.Delay(TimeSpan.FromSeconds(2)).Wait();

            try
            {
                PassiveListeningStartedEvent(this, null);
                Helpers.UiHelper.RunOnCoreDispatcherIfPossible(() => _awakeSpeechRecognizer.ContinuousRecognitionSession.StartAsync(SpeechContinuousRecognitionMode.Default), false);

            }
            catch { }

            Task.Delay(TimeSpan.FromSeconds(2)).Wait();
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
    }
}
