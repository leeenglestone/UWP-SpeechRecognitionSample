using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSpeechRecognitionSample.UwpApplication.Enums
{
    public enum ListeningState
    {
        NotStarted,
        PassiveListening,
        AnalysingSpeech,
        ActiveListening,
        Responding
    }
}
