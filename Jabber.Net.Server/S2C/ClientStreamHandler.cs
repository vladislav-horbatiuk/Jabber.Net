using agsXMPP;
using agsXMPP.protocol;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.session;
using agsXMPP.protocol.sasl;
using agsXMPP.protocol.stream;
using agsXMPP.protocol.stream.feature;
using agsXMPP.protocol.tls;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ClientStreamHandler : XmppHandler, IXmppHandler<Stream>
    {
        private readonly Jid domain;


        public ClientStreamHandler(Jid domain)
        {
            Args.NotNull(domain, "domain");
            this.domain = domain;
        }


        public XmppHandlerResult ProcessElement(Stream element, XmppSession session, XmppHandlerContext context)
        {
            if (element.To != domain)
            {
                return Error(session, StreamErrorCondition.HostUnknown);
            }

            var stream = new Stream
            {
                Id = session.Id,
                Prefix = Uri.PREFIX,
                Version = element.Version,
                From = element.To,
                Features = new Features { Prefix = Uri.PREFIX },
                Language = element.Language,
            };

            if (!session.Authenticated)
            {
                session.Jid = stream.From;
                session.Language = stream.Language;
                context.Sessions.OpenSession(session);

                stream.Features.Mechanisms = new Mechanisms();
                foreach (var m in context.Handlers.SupportedAuthMechanisms)
                {
                    stream.Features.Mechanisms.AddChild(m);
                }
                if (context.Handlers.SupportRegister)
                {
                    stream.Features.Register = new Register();
                }

                if (context.Handlers.SupportTls &&
                    session.Connection is IXmppTlsConnection && !((IXmppTlsConnection)session.Connection).TlsStarted)
                {
                    stream.Features.StartTls = new StartTls();
                }
            }
            else
            {
                if (context.Handlers.SupportBind)
                {
                    stream.Features.AddChild(new Bind());
                }
                if (context.Handlers.SupportSession)
                {
                    stream.Features.AddChild(new Session());
                }
            }

            return Send(session, stream);
        }
    }
}
