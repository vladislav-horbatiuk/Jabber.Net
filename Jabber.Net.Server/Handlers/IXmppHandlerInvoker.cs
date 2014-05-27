using System.Collections.Generic;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    interface IXmppHandlerInvoker
    {
        string HandlerId { get; }

        IEnumerable<XmppValidationAttribute> Validators { get; }

        XmppHandlerResult ProcessElement(Element e, XmppSession s, XmppHandlerContext c);
    }

}
