﻿using System;
using UwpSpeechRecognition.UserControlLibrary.Enums;
using Windows.UI.Xaml.Data;

namespace UwpSpeechRecognition.UserControlLibrary.Converters
{
    public class ListeningStateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (ListeningState)Enum.Parse(typeof(ListeningState), value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
