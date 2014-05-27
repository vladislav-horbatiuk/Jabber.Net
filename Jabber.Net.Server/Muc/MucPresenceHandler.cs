using agsXMPP.protocol.client;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Muc
{
    class MucPresenceHandler : XmppHandler,
        IXmppHandler<Presence>
    {
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            return Void();
        }
    }
}
