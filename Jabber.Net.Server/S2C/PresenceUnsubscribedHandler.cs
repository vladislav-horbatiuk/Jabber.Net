using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class PresenceUnsubscribedHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.unsubscribed)]
        [PresenceSubscription]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var result = Component();

            // contact server
            var ri = context.Storages.Users.GetRosterItem(session.Jid, element.To);
            if (ri != null && ri.HasFromSubscription())
            {
                ri.SetFromSubscription(false);
                context.Storages.Users.SaveRosterItem(session.Jid, ri);
                result.Add(new RosterPush(session.Jid, ri, context));
            }

            // user server
            ri = context.Storages.Users.GetRosterItem(element.To, session.Jid);
            if (ri != null && (ri.HasToSubscription() || ri.Ask == AskType.subscribe))
            {
                if (ri.HasToSubscription())
                {
                    ri.SetToSubscription(false);
                }
                else
                {
                    ri.Ask = AskType.NONE;
                }
                context.Storages.Users.SaveRosterItem(element.To, ri);

                foreach (var s in context.Sessions.GetSessions(session.Jid.BareJid))
                {
                    // unavailable
                    result.Add(Send(context.Sessions.GetSessions(element.To.BareJid), Presence.Unsubscribe(s.Jid, element.To)));
                }
                result.Add(Send(context.Sessions.GetSessions(element.To.BareJid), element));
                result.Add(new RosterPush(element.To, ri, context));
            }

            return result;
        }
    }
}
