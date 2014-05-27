using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class PresenceSubscribeHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.subscribe)]
        [PresenceSubscription]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var ri = context.Storages.Users.GetRosterItem(session.Jid, element.To);
            if (ri == null)
            {
                return Error(session, ErrorCondition.ItemNotFound, element);
            }
            if (ri.HasToSubscription())
            {
                return Send(session, Presence.Subscribed(element.To, element.From));
            }

            if (ri.Ask == AskType.NONE && !ri.HasToSubscription())
            {
                ri.Ask = AskType.subscribe;
                context.Storages.Users.SaveRosterItem(session.Jid, ri);
                return Component(
                    Send(context.Sessions.GetSessions(element.To.BareJid), element),
                    new RosterPush(session.Jid, ri, context));
            }

            return Void();
        }
    }
}
