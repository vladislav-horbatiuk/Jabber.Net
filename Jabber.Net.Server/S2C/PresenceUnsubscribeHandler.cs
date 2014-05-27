using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class PresenceUnsubscribeHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.unsubscribe)]
        [PresenceSubscription]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var result = Component();

            // user server
            var ri = context.Storages.Users.GetRosterItem(session.Jid, element.To);
            if (ri != null && (ri.HasToSubscription() || ri.Ask == AskType.subscribe))
            {
                if (ri.HasToSubscription() && ri.Ask == AskType.NONE)
                {
                    ri.SetToSubscription(false);
                }
                if (ri.Ask == AskType.subscribe)
                {
                    ri.Ask = AskType.NONE;
                }
                context.Storages.Users.SaveRosterItem(session.Jid, ri);

                result.Add(Send(context.Sessions.GetSessions(session.Jid.BareJid), element));
                result.Add(new RosterPush(session.Jid, ri, context));
            }

            // contact server
            ri = context.Storages.Users.GetRosterItem(element.To, session.Jid);
            if (ri != null && ri.HasFromSubscription())
            {
                ri.SetFromSubscription(false);
                context.Storages.Users.SaveRosterItem(element.To, ri);

                result.Add(Send(context.Sessions.GetSessions(element.To.BareJid), element));
                result.Add(new RosterPush(element.To, ri, context));
                foreach (var s in context.Sessions.GetSessions(element.To.BareJid))
                {
                    // unavailable
                    result.Add(Send(context.Sessions.GetSessions(session.Jid.BareJid), Presence.Unsubscribe(element.To, session.Jid)));
                }
            }

            
            return result;
        }
    }
}
