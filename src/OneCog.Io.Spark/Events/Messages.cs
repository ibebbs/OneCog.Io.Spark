using System.Text.RegularExpressions;

namespace OneCog.Io.Spark.Events
{
    public static class Messages
    {
        public static readonly string Stream = "ok";
        public static readonly string Event = "event";
        public static readonly string Data = "data";

        private const string HeaderGroup = "header";
        private const string EventGroup = "event";
        private const string DataGroup = "data";

        private static readonly Regex HeaderRegex = new Regex(@"^\s*:(?<header>ok)\s*$", RegexOptions.None);
        private static readonly Regex EventRegex = new Regex(@"^event:\s+(?<event>.*)\s*$", RegexOptions.None);
        private static readonly Regex DataRegex = new Regex(@"^data:\s+(?<data>.*)\s*$", RegexOptions.None);

        public static bool TryParseHeader(string message, out string header)
        {
            Match match = HeaderRegex.Match(message);

            if (match.Success && match.Groups[HeaderGroup].Success)
            {
                header = match.Groups[HeaderGroup].Value;
                return true;
            }
            else
            {
                header = null;
                return false;
            }
        }

        public static bool TryParseEvent(string message, out string name)
        {
            Match match = EventRegex.Match(message);

            if (match.Success && match.Groups[EventGroup].Success)
            {
                name = match.Groups[EventGroup].Value;
                return true;
            }
            else
            {
                name = null;
                return false;
            }
        }

        public static bool TryParseData(string message, out string data)
        {
            Match match = DataRegex.Match(message);

            if (match.Success && match.Groups[DataGroup].Success)
            {
                data = match.Groups[DataGroup].Value;
                return true;
            }
            else
            {
                data = null;
                return false;
            }
        }
    }
}
