using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.vcard;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServiceVCardHandler : XmppHandler, IXmppHandler<VcardIq>
    {
        private readonly ServiceInfo serviceInfo;


        public ServiceVCardHandler(ServiceInfo serviceInfo)
        {
            Args.NotNull(serviceInfo, "serviceInfo");
            this.serviceInfo = serviceInfo;
        }


        [IQ(IqType.get)]
        public XmppHandlerResult ProcessElement(VcardIq element, XmppSession session, XmppHandlerContext context)
        {
            element.Query = new Vcard
            {
                Fullname = serviceInfo.Name,
                Description = serviceInfo.Copyrigth,
                Url = serviceInfo.Url
            };
            return Send(session, element.ToResult());
        }
    }
}
