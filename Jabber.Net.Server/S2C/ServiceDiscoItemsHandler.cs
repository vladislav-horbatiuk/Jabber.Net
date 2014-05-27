using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServiceDiscoItemsHandler : XmppHandler, IXmppHandler<DiscoItemsIq>
    {
        private readonly ServiceInfo[] items;


        public ServiceDiscoItemsHandler(ServiceInfo[] items)
        {
            Args.NotNull(items, "items");
            this.items = items;
        }


        [IQ(IqType.get)]
        public XmppHandlerResult ProcessElement(DiscoItemsIq element, XmppSession session, XmppHandlerContext context)
        {
            foreach (var item in items)
            {
                element.Query.AddDiscoItem(item.Jid, item.Name);
            }
            return Send(session, element.ToResult());
        }
    }
}
