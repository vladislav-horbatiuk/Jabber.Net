using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Handlers.Results;

namespace Jabber.Net.Server.S2C
{
    class RosterPush : XmppComponentResult
    {
        public RosterPush(Jid to, RosterItem ri, XmppHandlerContext context)
        {
            Args.NotNull(to, "to");
            Args.NotNull(ri, "ri");
            Args.NotNull(context, "context");

            foreach (var s in context.Sessions.GetSessions(to.BareJid))
            {
                var push = new RosterIq { Type = IqType.set, From = to.BareJid, To = s.Jid, Query = new Roster() };
                push.Query.AddRosterItem(ri);
                Add(new XmppSendResult(s, push, false));
            }
        }
    }
}
