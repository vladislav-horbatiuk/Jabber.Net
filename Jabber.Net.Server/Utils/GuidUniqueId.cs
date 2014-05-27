using System;

namespace Jabber.Net.Server.Utils
{
    class GuidUniqueId : IUniqueId
    {
        public string CreateId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
