using System;
using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers.Results;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class XmppValidationAttribute : Attribute
    {
        public abstract XmppHandlerResult ValidateElement(Element element, XmppSession session, XmppHandlerContext context);


        protected XmppHandlerResult Success()
        {
            return null;
        }

        protected XmppHandlerResult Error(XmppSession session, ErrorCondition error, Stanza stanza)
        {
            return new XmppErrorResult(session, new JabberStanzaException(error, stanza));
        }

        protected XmppHandlerResult Error(XmppSession session, StreamErrorCondition error)
        {
            return new XmppErrorResult(session, new JabberStreamException(error));
        }

        protected XmppHandlerResult Fail()
        {
            return new XmppComponentResult();
        }
    }
}
