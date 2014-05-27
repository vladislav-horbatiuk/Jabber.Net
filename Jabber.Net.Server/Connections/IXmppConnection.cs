using System;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppConnection
    {
        string SessionId
        {
            get;
            set;
        }

        void BeginReceive(XmppHandlerManager handlerManager);

        void Send(Element element, Action<Element> onerror);

        void Reset();

        void Close();
    }
}
