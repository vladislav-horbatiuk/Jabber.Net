using System;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.version;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServiceVersionHandler : XmppHandler, IXmppHandler<VersionIq>
    {
        private readonly ServiceInfo serviceInfo;


        public ServiceVersionHandler(ServiceInfo serviceInfo)
        {
            Args.NotNull(serviceInfo, "serviceInfo");
            this.serviceInfo = serviceInfo;
        }


        [IQ(IqType.get)]
        public XmppHandlerResult ProcessElement(VersionIq element, XmppSession session, XmppHandlerContext context)
        {
            element.Query.Os = Environment.OSVersion.ToString();
            element.Query.Name = serviceInfo.Name;
            element.Query.Ver = "1.0";
            return Send(session, element.ToResult());
        }
    }
}
