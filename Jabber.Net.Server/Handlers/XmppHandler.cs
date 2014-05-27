using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP.protocol;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers.Results;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandler
    {
        private static readonly IUniqueId id = new RandomUniqueId();


        protected XmppHandlerResult Send(XmppSession session, Element element)
        {
            return Send(session, element, false);
        }

        protected XmppHandlerResult Send(XmppSession session, Element element, bool offline)
        {
            return new XmppSendResult(session, element, offline);
        }

        protected XmppHandlerResult Send(IEnumerable<XmppSession> sessions, params Element[] elements)
        {
            return Send(sessions, false, elements);
        }

        protected XmppHandlerResult Send(IEnumerable<XmppSession> sessions, bool offline, params Element[] elements)
        {
            return Component((from s in sessions from e in elements select Send(s, e, offline)).ToArray());
        }


        protected XmppHandlerResult Error(XmppSession session, StreamErrorCondition error)
        {
            return Error(session, new JabberStreamException(error));
        }

        protected XmppHandlerResult Error(XmppSession session, FailureCondition error)
        {
            return Error(session, new JabberSaslException(error));
        }

        protected XmppHandlerResult Error(XmppSession session, ErrorCondition error, Stanza stanza)
        {
            return Error(session, error, stanza, null);
        }

        protected XmppHandlerResult Error(XmppSession session, ErrorCondition error, Stanza stanza, string message)
        {
            return Error(session, new JabberStanzaException(error, stanza, message));
        }

        protected XmppHandlerResult Error(XmppSession session, Exception error)
        {
            return new XmppErrorResult(session, error);
        }


        protected XmppHandlerResult Close(XmppSession session)
        {
            return new XmppCloseResult(session);
        }

        protected XmppComponentResult Component(params XmppHandlerResult[] results)
        {
            return new XmppComponentResult(results);
        }

        protected XmppHandlerResult Void()
        {
            return null;
        }

        protected XmppHandlerResult Request(XmppSession session, IQ iq, XmppHandlerResult errorResponse)
        {
            return new XmppRequestResult(session, iq, errorResponse);
        }

        protected XmppHandlerResult Process(XmppSession session, Element element)
        {
            return new XmppProcessResult(session, element);
        }

        protected string CreateId()
        {
            return id.CreateId();
        }
    }
}
