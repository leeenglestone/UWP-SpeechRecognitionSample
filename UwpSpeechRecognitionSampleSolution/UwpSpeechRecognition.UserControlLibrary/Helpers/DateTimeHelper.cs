using System;

namespace UwpSpeechRecognition.UserControlLibrary.Helpers
{
    public class DateTimeHelper
    {
        public static string ToHumanReadableTime(DateTime dateTime)
        {
            // The output should be something like..
            // Seven thirty six PM

            var currentDateTime = DateTime.Now;

            return NumberHelper.NumberToWords(currentDateTime.Hour) 
                + " " 
                + NumberHelper.NumberToWords(currentDateTime.Minute);
        }
    }
}
