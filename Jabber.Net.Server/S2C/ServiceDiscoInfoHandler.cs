using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServiceDiscoInfoHandler : XmppHandler, IXmppHandler<DiscoInfoIq>
    {
        private readonly ServiceInfo serviceInfo;

        
        public ServiceDiscoInfoHandler(ServiceInfo serviceInfo)
        {
            Args.NotNull(serviceInfo, "serviceInfo");
            this.serviceInfo = serviceInfo;
        }

        [IQ(IqType.get)]
        public XmppHandlerResult ProcessElement(DiscoInfoIq element, XmppSession session, XmppHandlerContext context)
        {
            element.Query.AddIdentity(serviceInfo.Category, serviceInfo.Type, serviceInfo.Name);
            foreach (var feature in serviceInfo.Features.Reverse())
            {
                element.Query.AddFeature(feature);
            }
            return Send(session, element.ToResult());
        }
    }
}
