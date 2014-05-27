using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public interface IXmppHandler<T> where T : Element
    {
        XmppHandlerResult ProcessElement(T element, XmppSession session, XmppHandlerContext context);
    }
}
