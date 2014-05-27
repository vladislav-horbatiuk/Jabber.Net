using System;
using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.last;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.S2C
{
    class ClientLastHandler : XmppHandler, IXmppHandler<LastIq>
    {
        [IQ(IqType.get, IqType.result, IqType.error)]
        public XmppHandlerResult ProcessElement(LastIq element, XmppSession session, XmppHandlerContext context)
        {
            if (!element.HasTo)
            {
                return Error(session, ErrorCondition.BadRequest, element);
            }

            if (element.Type == IqType.get)
            {
                var ri = context.Storages.Users.GetRosterItem(session.Jid, element.To);
                if (ri == null || !ri.HasToSubscription())
                {
                    return Error(session, ErrorCondition.Forbidden, element);
                }
            }

            if (element.Type == IqType.get && element.To.IsBare)
            {
                if (context.Sessions.GetSessions(element.To).Any())
                {
                    element.Query.Seconds = 0;
                }
                else
                {
                    var last = context.Storages.Elements.GetElement(element.To, agsXMPP.Uri.IQ_LAST) as Last;
                    if (last != null)
                    {
                        element.Query.Seconds = UnixDateTime.ToUnix(DateTime.UtcNow) - last.Seconds;
                        element.Query.Value = last.Value;
                    }
                    else
                    {
                        return Error(session, ErrorCondition.ItemNotFound, element);
                    }
                }
                return Send(session, element.ToResult());
            }
            else if (element.To.IsFull)
            {
                var to = context.Sessions.GetSession(element.To);
                if (to == null)
                {
                    return element.Type == IqType.get ? Error(session, ErrorCondition.RecipientUnavailable, element) : Void();
                }

                return Request(to, element, Error(session, ErrorCondition.RecipientUnavailable, element));
            }

            return element.Type == IqType.get ? Error(session, ErrorCondition.ServiceUnavailable, element) : Void();
        }
    }
}
