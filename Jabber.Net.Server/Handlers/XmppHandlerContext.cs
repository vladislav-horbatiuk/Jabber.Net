using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Storages;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerContext
    {
        public XmppHandlerManager Handlers
        {
            get;
            private set;
        }

        public XmppSessionManager Sessions
        {
            get;
            private set;
        }

        public XmppStorageManager Storages
        {
            get;
            private set;
        }

        public XmppHandlerContext(XmppHandlerManager handlers, IXmppResolver resolver)
        {
            Args.NotNull(resolver, "resolver");
            Args.NotNull(handlers, "handlers");

            Handlers = handlers;
            Sessions = resolver.Resolve<XmppSessionManager>();
            Storages = resolver.Resolve<XmppStorageManager>();
        }
    }
}
