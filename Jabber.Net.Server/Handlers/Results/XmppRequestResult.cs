using System;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppRequestResult : XmppHandlerResult
    {
        private readonly IQ iq;
        private readonly XmppHandlerResult errorResponse;


        public XmppRequestResult(XmppSession session, IQ iq, XmppHandlerResult errorResponse)
            : base(session)
        {
            Args.NotNull(iq, "iq");

            this.iq = iq;
            this.errorResponse = errorResponse;
        }


        public override void Execute(XmppHandlerContext context)
        {
            Action<Element> onerror = null;
            if (errorResponse != null && (iq.Type == IqType.get || iq.Type == IqType.set))
            {
                onerror = _ => context.Handlers.ProcessResult(errorResponse);
            }
            Session.Connection.Send(iq, onerror);
        }
    }
}
