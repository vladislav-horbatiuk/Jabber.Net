using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP.protocol.iq.disco;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Storages;

namespace Jabber.Net.Server.Muc
{
    class MucDiscoItemsHandler : XmppHandler,
        IXmppHandler<DiscoItems>
    {
        public XmppHandlerResult ProcessElement(DiscoItems element, XmppSession session, XmppHandlerContext context)
        {
            return Send(session, element.ToResult());
        }
    }
}
