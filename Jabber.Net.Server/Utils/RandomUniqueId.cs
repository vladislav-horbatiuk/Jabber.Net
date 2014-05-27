using System;

namespace Jabber.Net.Server.Utils
{
    class RandomUniqueId : IUniqueId
    {
        private static readonly Random random = new Random();


        public string CreateId()
        {
            return random.Next(UInt16.MaxValue).ToString();
        }
    }
}
