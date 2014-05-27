using System;

namespace Jabber.Net.Server
{
    static class Args
    {
        public static void NotNull(object arg, string name)
        {
            Requires<ArgumentNullException>(arg != null, name);
        }

        public static void Requires<TException>(bool condition, string message) where TException : Exception
        {
            if (!condition)
            {
                try
                {
                    throw (TException)Activator.CreateInstance(typeof(TException), message);
                }
                catch (MissingMethodException)
                {
                    throw new ArgumentException(message);
                }
            }
        }
    }
}
