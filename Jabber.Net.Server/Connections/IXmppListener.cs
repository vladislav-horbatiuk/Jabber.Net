using System;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppListener
    {
        void StartListen(Action<IXmppConnection> newConnection);

        void StopListen();
    }
}
