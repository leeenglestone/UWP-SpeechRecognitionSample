using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpSpeechRecognition.UserControlLibrary.Enums;
using Windows.UI.Xaml.Data;

namespace UwpSpeechRecognitionSample.UwpApplication.Converters
{
    public class ListeningStateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ListeningState listeningState = (ListeningState)Enum.Parse(typeof(ListeningState), value.ToString());
            return listeningState;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
