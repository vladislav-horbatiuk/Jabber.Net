using System;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppErrorResult : XmppHandlerResult
    {
        private readonly XmppHandlerResult result;


        public XmppErrorResult(XmppSession session, Exception error)
            : base(XmppSession.Empty)
        {
            Args.NotNull(error, "error");

            var je = (error as JabberException) ?? new JabberStreamException(error);
            XmppHandlerResult send = new XmppSendResult(session, je.ToElement(), false);
            result = je.CloseStream ? new XmppComponentResult(send, new XmppCloseResult(session)) : send;
        }

        public override void Execute(XmppHandlerContext context)
        {
            context.Handlers.ProcessResult(result);
        }
    }
}
