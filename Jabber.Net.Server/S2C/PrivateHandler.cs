using System.Collections.Generic;
using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.@private;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class PrivateHandler : XmppHandler,
        IXmppHandler<PrivateIq>
    {
        [IQ(IqType.get, IqType.set)]
        public XmppHandlerResult ProcessElement(PrivateIq element, XmppSession session, XmppHandlerContext context)
        {
            if (element.HasTo && element.To != session.Jid)
            {
                return Error(session, ErrorCondition.Forbidden, element);
            }
            if (!element.HasChildElements)
            {
                return Error(session, ErrorCondition.BadRequest, element);
            }

            var elements = element.Query.ChildNodes.OfType<Element>().ToArray();
            element.Query.RemoveAllChildNodes();

            if (element.Type == IqType.get)
            {
                foreach (var e in elements)
                {
                    var restored = context.Storages.Elements.GetElement(session.Jid, e.TagName + e.Namespace);
                    if (restored != null)
                    {
                        element.Query.AddChild(e);
                    }
                }
            }
            else
            {
                foreach (var e in elements)
                {
                    context.Storages.Elements.SaveElement(session.Jid, e.TagName + e.Namespace, e);
                }
            }

            return Send(session, element.ToResult());
        }
    }
}
