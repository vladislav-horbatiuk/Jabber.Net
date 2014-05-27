using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class PresenceSubscribedHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.subscribed)]
        [PresenceSubscription]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var result = Component();

            var userItem = context.Storages.Users.GetRosterItem(element.To, session.Jid);
            if (userItem != null && !userItem.HasToSubscription() && userItem.Ask == AskType.subscribe)
            {
                var contactItem = context.Storages.Users.GetRosterItem(session.Jid, element.To);
                if (contactItem == null)
                {
                    contactItem = new RosterItem(element.To);
                    if (element.Nickname != null && !string.IsNullOrEmpty(element.Nickname.Value))
                    {
                        contactItem.Name = element.Nickname.Value;
                    }
                }
                if (!contactItem.HasFromSubscription())
                {
                    contactItem.SetFromSubscription(true);
                    context.Storages.Users.SaveRosterItem(session.Jid, contactItem);
                    result.Add(new RosterPush(session.Jid, contactItem, context));
                }

                userItem.SetToSubscription(true);
                userItem.Ask = AskType.NONE;
                context.Storages.Users.SaveRosterItem(element.To, userItem);

                result.Add(Send(context.Sessions.GetSessions(element.To.BareJid), element));
                result.Add(new RosterPush(element.To, userItem, context));
                foreach (var s in context.Sessions.GetSessions(session.Jid.BareJid))
                {
                    // available
                    result.Add(Send(context.Sessions.GetSessions(element.To.BareJid), Presence.Available(s.Jid, element.To)));
                }
            }

            return result;
        }
    }
}
