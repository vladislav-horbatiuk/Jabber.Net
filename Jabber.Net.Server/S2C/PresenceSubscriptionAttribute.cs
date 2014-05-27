using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class PresenceSubscriptionAttribute : XmppValidationAttribute
    {
        public override XmppHandlerResult ValidateElement(Element element, XmppSession session, XmppHandlerContext context)
        {
            var presence = element as Presence;
            if (!presence.HasTo)
            {
                return Fail();
            }
            if (presence.To.IsFull)
            {
                presence.To = presence.To.BareJid;
            }
            presence.From = session.Jid.BareJid;
            if (presence.From == presence.To)
            {
                return Fail();
            }

            return Success();
        }
    }
}
