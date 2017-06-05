using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSpeechRecognition.UserControlLibrary.EventArgs
{
    public class PhraseRecognisedEventArgs : System.EventArgs
    {
        public string RecognisedPhrase { get; set; }
    }
}
