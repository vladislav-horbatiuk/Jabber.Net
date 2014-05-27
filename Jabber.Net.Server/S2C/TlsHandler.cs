using System.Net;
using System.Security.Cryptography.X509Certificates;
using agsXMPP.protocol.tls;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class TlsHandler : XmppHandler,
        IXmppHandler<StartTls>,
        IXmppHandler<Proceed>,
        IXmppRegisterHandler
    {
        private readonly X509Certificate certificate;


        public TlsHandler(string certificatefile)
        {
            certificate = X509Certificate.CreateFromCertFile(certificatefile);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
        }


        public void OnRegister(XmppHandlerContext context)
        {
            context.Handlers.SupportTls = true;
        }


        public XmppHandlerResult ProcessElement(StartTls element, XmppSession session, XmppHandlerContext context)
        {
            return Component(Send(session, new Proceed()), Process(session, new Proceed()));
        }

        public XmppHandlerResult ProcessElement(Proceed element, XmppSession session, XmppHandlerContext context)
        {
            ((IXmppTlsConnection)session.Connection).TlsStart(certificate);
            return Void();
        }
    }
}
