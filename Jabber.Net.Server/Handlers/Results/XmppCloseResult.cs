using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppCloseResult : XmppHandlerResult
    {
        public XmppCloseResult(XmppSession session)
            : base(session)
        {
        }


        public override void Execute(XmppHandlerContext context)
        {
            context.Sessions.CloseSession(Session.Id);
        }
    }
}
