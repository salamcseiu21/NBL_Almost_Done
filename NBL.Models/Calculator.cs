using System;

namespace NBL.Models
{
    public static class Calculator
    {
        public static string TimeDuration(DateTime startDateTime, DateTime endDateTime)
        {
            TimeSpan timeSpan = endDateTime - startDateTime;
            return $"{timeSpan}";
        }
    }
}
