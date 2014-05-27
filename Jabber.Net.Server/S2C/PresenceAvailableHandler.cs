using System.Linq;
using agsXMPP.protocol.client;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Storages;

namespace Jabber.Net.Server.S2C
{
    class PresenceAvailableHandler : XmppHandler, IXmppHandler<Presence>
    {
        [PresenceFilter(PresenceType.available)]
        public XmppHandlerResult ProcessElement(Presence element, XmppSession session, XmppHandlerContext context)
        {
            var result = Component();
            if (!element.HasTo)
            {
                if (!session.Available)
                {
                    session.Presence = element;

                    // send offline stanzas
                    foreach (var jid in context.Storages.Users.GetAskers(session.Jid))
                    {
                        result.Add(Send(session, Presence.Subscribe(jid, session.Jid)));
                    }
                    foreach (var e in context.Storages.Elements.GetOfflines(session.Jid))
                    {
                        result.Add(Send(session, e, true));
                    }
                    context.Storages.Elements.RemoveOfflines(session.Jid);

                    // send presences from roster
                    foreach (var ri in context.Storages.Users.GetRosterItems(session.Jid))
                    {
                        if (ri.HasFromSubscription())
                        {
                            foreach (var s in context.Sessions.GetSessions(ri.Jid.BareJid).Where(s => s.Available))
                            {
                                if (s.Presence != null)
                                {
                                    var p = (Presence)s.Presence.Clone();
                                    p.To = session.Jid;
                                    result.Add(Send(session, p));
                                }
                            }
                        }
                    }
                }

                // send to itself available resource
                foreach (var s in context.Sessions.GetSessions(element.From.BareJid).Where(s => s.Available))
                {
                    var p = (Presence)element.Clone();
                    p.To = s.Jid;
                    result.Add(Send(s, p));
                }

                // broadcast to subscribers
                foreach (var to in context.Storages.Users.GetSubscribers(session.Jid))
                {
                    foreach (var s in context.Sessions.GetSessions(to.BareJid).Where(s => s.Available))
                    {
                        var p = (Presence)element.Clone();
                        p.To = s.Jid;
                        result.Add(Send(s, p));
                    }
                }
            }
            return result;
        }
    }
}
