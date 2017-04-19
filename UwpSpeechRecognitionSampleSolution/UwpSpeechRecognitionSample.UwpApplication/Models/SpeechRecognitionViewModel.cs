using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSpeechRecognitionSample.UwpApplication.Models
{
    public class SpeechRecognitionViewModel
    {
        public string ListeningState { get; set; }
        public string ActionResult { get; set; }
        public string RecognisedPhrase { get; set; }

    }
}
