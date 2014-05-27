using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public interface IXmppCloseHandler
    {
        XmppHandlerResult OnClose(XmppSession session, XmppHandlerContext context);
    }
}
