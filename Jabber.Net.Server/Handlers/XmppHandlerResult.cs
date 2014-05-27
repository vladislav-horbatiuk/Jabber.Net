using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public abstract class XmppHandlerResult
    {
        protected XmppSession Session
        {
            get;
            private set;
        }


        public XmppHandlerResult(XmppSession session)
        {
            Args.NotNull(session, "session");

            Session = session;
        }


        public abstract void Execute(XmppHandlerContext context);
    }
}
