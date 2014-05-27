using System;

namespace Jabber.Net.Server.Utils
{
    static class UnixDateTime
    {
        private readonly static DateTime start = new DateTime(1970, 1, 1);


        public static DateTime FromUnix(int seconds)
        {
            return start.Add(TimeSpan.FromSeconds(seconds));
        }

        public static int ToUnix(DateTime dateTime)
        {
            return (int)(dateTime - start).TotalSeconds;
        }
    }
}
