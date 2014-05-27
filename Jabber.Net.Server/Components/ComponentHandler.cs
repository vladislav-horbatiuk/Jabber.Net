using agsXMPP;
using agsXMPP.protocol;
using agsXMPP.protocol.component;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Components
{
    class ComponentHandler : XmppHandler,
        IXmppHandler<Stream>,
        IXmppHandler<Handshake>,
        IXmppHandler<agsXMPP.protocol.Base.Stanza>,
        IXmppHandler<agsXMPP.protocol.component.Error>
    {
        private readonly Jid domain;
        private readonly string secret;


        public ComponentHandler(Jid domain, string secret)
        {
            Args.NotNull(domain, "domain");

            this.domain = domain;
            this.secret = secret;
        }


        public XmppHandlerResult ProcessElement(Stream element, XmppSession session, XmppHandlerContext context)
        {
            if (element.To != domain)
            {
                return Error(session, StreamErrorCondition.HostUnknown);
            }

            element.SwitchDirection();
            element.Id = CreateId();
            element.To = null;

            session.Jid = domain;
            context.Sessions.OpenSession(session);
            session.AuthData = element.Id;

            return Send(session, element);
        }


        public XmppHandlerResult ProcessElement(Handshake element, XmppSession session, XmppHandlerContext context)
        {
            var test = new Handshake(secret, session.AuthData as string);
            if (element.Value != test.Value)
            {
                return Error(session, StreamErrorCondition.NotAuthorized);
            }

            session.Authenticate(null);
            session.Bind(null);
            return Send(session, new Handshake());
        }

        public XmppHandlerResult ProcessElement(agsXMPP.protocol.Base.Stanza element, XmppSession session, XmppHandlerContext context)
        {
            if (!element.HasTo)
            {
                return Error(session, agsXMPP.protocol.client.ErrorCondition.BadRequest, element);
            }

            if (element.To.Server == domain.Server)
            {
                var to = context.Sessions.GetSession(domain);
                return to != null ? Send(to, element) : Error(session, agsXMPP.protocol.client.ErrorCondition.ServiceUnavailable, element);
            }

            return Send(context.Sessions.GetSessions(element.To), element);
        }

        public XmppHandlerResult ProcessElement(agsXMPP.protocol.component.Error element, XmppSession session, XmppHandlerContext context)
        {
            return Void();
        }
    }
}
