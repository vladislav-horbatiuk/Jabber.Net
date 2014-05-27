using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ClientRosterHandler : XmppHandler, IXmppHandler<RosterIq>
    {
        [IQ(IqType.get, IqType.set)]
        public XmppHandlerResult ProcessElement(RosterIq element, XmppSession session, XmppHandlerContext context)
        {
            var to = element.HasTo ? element.To : session.Jid;
            if (to != session.Jid)
            {
                return Error(session, ErrorCondition.Forbidden, element);
            }

            if (element.Type == IqType.get)
            {
                foreach (var ri in context.Storages.Users.GetRosterItems(to))
                {
                    element.Query.AddRosterItem(ri);
                }
                session.RosterRequest();
                return Send(session, element.ToResult());
            }
            else
            {
                if (element.Query.Items.Count() != 1)
                {
                    return Error(session, ErrorCondition.BadRequest, element);
                }

                var ri = element.Query.Items.ElementAt(0);
                var result = Component(new RosterPush(session.Jid, ri, context));
                element.Query.Remove();
                result.Add(Send(session, element.ToResult()));

                if (ri.Subscription == SubscriptionType.remove)
                {
                    var item = context.Storages.Users.GetRosterItem(to, ri.Jid);
                    if (item != null)
                    {
                        context.Storages.Users.RemoveRosterItem(to, ri.Jid);

                        if (item.HasToSubscription())
                        {
                            result.Add(Process(session, Presence.Unsubscribe(session.Jid.BareJid, ri.Jid.BareJid)));
                        }
                        if (item.HasFromSubscription())
                        {
                            result.Add(Process(session, Presence.Unsubscribed(session.Jid.BareJid, ri.Jid.BareJid)));
                        }
                    }
                    else
                    {
                        return Error(session, ErrorCondition.ItemNotFound, element);
                    }
                }
                else
                {
                    if (session.Jid.BareJid == ri.Jid.BareJid)
                    {
                        return Error(session, ErrorCondition.Conflict, element);
                    }

                    var old = context.Storages.Users.GetRosterItem(to, ri.Jid);
                    if (old != null)
                    {
                        ri.Subscription = old.Subscription;
                        ri.Ask = old.Ask;
                    }
                    else
                    {
                        ri.Subscription = SubscriptionType.none;
                        ri.Ask = AskType.NONE;
                    }
                    context.Storages.Users.SaveRosterItem(to, ri);
                }

                return result;
            }
        }
    }
}
