using System;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.last;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServiceLastHandler : XmppHandler, IXmppHandler<LastIq>
    {
        private static readonly DateTime started = DateTime.UtcNow;


        [IQ(IqType.get)]
        public XmppHandlerResult ProcessElement(LastIq element, XmppSession session, XmppHandlerContext context)
        {
            element.Query.Seconds = (int)(DateTime.UtcNow - started).TotalSeconds;
            return Send(session, element.ToResult());
        }
    }
}
