// This code & software is licensed under the Creative Commons license. You can't use AMWE trademark 
// You can use & improve this code by keeping this comments
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;

namespace m3md2
{
    public static class Parser
    {
        public static string RelativeTime(DateTime yourDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.Now.Ticks - yourDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "1 s" : ts.Seconds + " s";

            if (delta < 2 * MINUTE)
                return "1 m";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " m";

            if (delta < 90 * MINUTE)
                return "1 h";

            if (delta < 24 * HOUR)
                return ts.Hours + " h";

            if (delta < 48 * HOUR)
                return "1 d";

            if (delta < 30 * DAY)
                return ts.Days + " d";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        public static TimeDescription GetTimeDescription(DateTime dateTime)
        {
            if (dateTime.Hour <= 3)
            {
                return TimeDescription.Night;
            }
            else if (dateTime.Hour <= 11)
            {
                return TimeDescription.Morning;
            }
            else if (dateTime.Hour <= 16)
            {
                return TimeDescription.Afternoon;
            }
            else if (dateTime.Hour <= 23)
            {
                return TimeDescription.Evening;
            }
            throw new ArgumentException($"(17.1) Получено неожиданное значение для datetime.Hour: {dateTime.Hour}");
        }

        public static string GetWelcomeLabel(TimeDescription timedesc)
        {
            return timedesc switch
            {
                TimeDescription.Morning => "Доброе утро",
                TimeDescription.Afternoon => "Добрый день",
                TimeDescription.Evening => "Добрый вечер",
                TimeDescription.Night => "Доброй ночи",
                _ => throw new ArgumentException($"(17) Значение определено неверно: timedesc {timedesc}"),
            };
        }

        public static string GetUntilOrEmpty(this string text, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
    }

    public enum TimeDescription
    {
        Night,
        Morning,
        Afternoon,
        Evening
    }
}
