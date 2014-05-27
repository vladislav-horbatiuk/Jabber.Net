using System.Threading;

namespace Jabber.Net.Server.Utils
{
    class IncrementalUniqueId : IUniqueId
    {
        private static int counter = 0;


        public string CreateId()
        {
            return Interlocked.Increment(ref counter).ToString();
        }
    }
}
