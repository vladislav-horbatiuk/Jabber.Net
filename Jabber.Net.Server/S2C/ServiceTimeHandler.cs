using System;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.time;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class ServiceTimeHandler : XmppHandler, IXmppHandler<EntityTimeIq>
    {
        [IQ(IqType.get)]
        public XmppHandlerResult ProcessElement(EntityTimeIq element, XmppSession session, XmppHandlerContext context)
        {
            var tzo = TimeZoneInfo.Local.BaseUtcOffset;
            element.Time.Tzo = tzo.Hours.ToString("+00;-00") + tzo.Minutes.ToString(":00");
            element.Time.Utc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            return Send(session, element.ToResult());
        }
    }
}
