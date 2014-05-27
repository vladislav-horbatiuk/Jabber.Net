using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppProcessResult : XmppHandlerResult
    {
        private readonly Element element;


        public XmppProcessResult(XmppSession session, Element element)
            : base(session)
        {
            Args.NotNull(element, "element");
            this.element = element;
        }

        public override void Execute(XmppHandlerContext context)
        {
            context.Handlers.ProcessElement(Session.Connection, element);
        }
    }
}
