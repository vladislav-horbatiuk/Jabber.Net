using System;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public interface IXmppErrorHandler
    {
        XmppHandlerResult OnError(Exception error, XmppSession session, XmppHandlerContext context);
    }
}
