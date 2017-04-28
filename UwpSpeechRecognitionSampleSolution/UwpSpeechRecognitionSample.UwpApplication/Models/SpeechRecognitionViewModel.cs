﻿using System;
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

                    case ListeningState.ActiveListening:
                        StopListeningButtonVisibility = Visibility.Visible;
                        StartListeningButtonVisibility = Visibility.Collapsed;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Initialize()
        {
            if (!(await CheckForMicrophonePermission()))
                return;

            _awakeSpeechRecognizer = new SpeechRecognizer();
            _awakeSpeechRecognizer.Constraints.Add(new SpeechRecognitionListConstraint(new List<String>() { "Oracle" }, "Awake"));

            var result = await _awakeSpeechRecognizer.CompileConstraintsAsync();

            if (result.Status != SpeechRecognitionResultStatus.Success)
                return;

            _awakeSpeechRecognizer.ContinuousRecognitionSession.ResultGenerated += AwakeContinuousRecognitionSession_ResultGenerated;
            await _awakeSpeechRecognizer.ContinuousRecognitionSession.StartAsync(SpeechContinuousRecognitionMode.Default);

            _commandSpeechRecognizer = new SpeechRecognizer();
            result = await _commandSpeechRecognizer.CompileConstraintsAsync();
            _commandSpeechRecognizer.HypothesisGenerated += _commandSpeechRecognizer_HypothesisGenerated;
        }

        private void _commandSpeechRecognizer_HypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs args)
        {
            Helpers.Helpers.RunOnCoreDispatcherIfPossible(() => RecognisedPhrase = args.Hypothesis.Text.ToLower(), false);
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

        private void AwakeContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            if (args.Result.Confidence == SpeechRecognitionConfidence.High || args.Result.Confidence == SpeechRecognitionConfidence.Medium)
            {
                Helpers.Helpers.RunOnCoreDispatcherIfPossible(() => WakeUpAndListen(), false);
            }
        }

        private async Task WakeUpAndListen()
        {
            // Stop awake listener
            //await _awakeSpeechRecognizer.ContinuousRecognitionSession.CancelAsync();

            ListeningState = ListeningState.ActiveListening;

            // Do some stuff
            // Start timer
            //var timer = new DispatcherTimer();
            //timer.
            // Start command listener
            // Stop timer
            // Stop command listener

            // Start awake listener
            //await _awakeSpeechRecognizer.ContinuousRecognitionSession.StartAsync();

            //ListeningState = ListeningState.PassiveListening;
        }
    }
}
