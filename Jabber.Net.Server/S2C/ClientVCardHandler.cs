using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.vcard;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ClientVCardHandler : XmppHandler, IXmppHandler<VcardIq>
    {
        [IQ(IqType.get, IqType.set)]
        public XmppHandlerResult ProcessElement(VcardIq element, XmppSession session, XmppHandlerContext context)
        {
            var to = element.HasTo ? element.To : session.Jid;
            if (element.Type == IqType.get)
            {
                element.Vcard = context.Storages.Users.GetVCard(to.User) ?? new Vcard();
            }
            else
            {
                if (session.Jid != to)
                {
                    return Error(session, ErrorCondition.Forbidden, element);
                }
                context.Storages.Users.SetVCard(to.User, element.Vcard);
                element.Vcard.Remove();
            }
            return Send(session, element.ToResult());
        }
    }
}
