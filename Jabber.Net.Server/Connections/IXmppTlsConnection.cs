using System.Security.Cryptography.X509Certificates;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppTlsConnection
    {
        bool TlsStarted
        {
            get;
        }

        void TlsStart(X509Certificate certificate);
    }
}
