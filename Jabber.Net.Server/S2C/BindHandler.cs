using System;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.bind;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class BindHandler : XmppHandler, IXmppHandler<BindIq>, IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.Handlers.SupportBind = true;
        }

        [IQ(IqType.set)]
        public XmppHandlerResult ProcessElement(BindIq element, XmppSession session, XmppHandlerContext context)
        {
            if (element.Query.TagName.Equals("bind", StringComparison.OrdinalIgnoreCase))
            {
                if (session.Binded)
                {
                    return Error(session, ErrorCondition.Conflict, element);
                }

                session.Bind(!string.IsNullOrEmpty(element.Query.Resource) ? element.Query.Resource : session.Jid.User);
                element.From = element.To = null;
                element.Query = new Bind(session.Jid);
                var result = Component(Send(session, element.ToResult()));
                foreach (var s in context.Sessions.GetSessions(session.Jid))
                {
                    if (!session.Equals(s))
                    {
                        result.Add(Close(s));
                    }
                }
                return result;
            }
            else
            {
                var resource = element.Query.Resource;
                return session.Jid.Resource == resource ? Close(session) : Error(session, ErrorCondition.ItemNotFound, element);
            }
        }
    }
}
