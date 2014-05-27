using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class IQAttribute : XmppValidationAttribute
    {
        private readonly IqType[] allowed;

        public ErrorCondition ErrorCondition
        {
            get;
            set;
        }


        public IQAttribute(params IqType[] allowed)
        {
            this.allowed = allowed;
        }

        public override XmppHandlerResult ValidateElement(Element element, XmppSession session, XmppHandlerContext context)
        {
            var iq = element as IQ;
            if (iq != null)
            {
                if (!allowed.Contains(iq.Type))
                {
                    if (iq.Type == IqType.get || iq.Type == IqType.set)
                    {
                        return Error(session, ErrorCondition, iq);
                    }
                    else
                    {
                        return Fail();
                    }
                }
            }
            return Success();
        }
    }
}
