using System;
using System.Diagnostics;

namespace Jabber.Net.Server
{
    static class Log
    {
        [Conditional("DEBUG")]
        public static void Information(string message)
        {
            Information(message, null);
        }

        [Conditional("DEBUG")]
        public static void Information(string format, params object[] args)
        {
            
            Trace.TraceInformation(AddInfo(format), args);
        }

        [Conditional("DEBUG")]
        public static void Warning(string message)
        {
            Warning(message, null);
        }

        [Conditional("DEBUG")]
        public static void Warning(string format, params object[] args)
        {
            Trace.TraceWarning(AddInfo(format), args);
        }

        public static void Error(Exception error)
        {
            Error(error.ToString(), null);
        }

        public static void Error(string format, params object[] args)
        {
            Trace.TraceError(AddInfo(format), args);
        }

        private static string AddInfo(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return format;
            }

            format = DateTime.Now.ToString("[HH:mm:ss.ffffff]") + " " + format;
            return format;
        }
    }
}
