using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class PresenceFilterAttribute : XmppValidationAttribute
    {
        private readonly PresenceType allowed;


        public PresenceFilterAttribute(PresenceType allowed)
        {
            this.allowed = allowed;
        }

        public override XmppHandlerResult ValidateElement(Element element, XmppSession session, XmppHandlerContext context)
        {
            var presence = element as Presence;
            if (presence.Type != allowed)
            {
                return Fail();
            }
            return Success();
        }
    }
}
