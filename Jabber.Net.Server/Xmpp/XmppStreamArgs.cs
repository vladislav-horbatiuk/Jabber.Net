using System;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Xmpp
{
    class XmppStreamArgs : EventArgs
    {
        public XmppStreamState State
        {
            get;
            private set;
        }

        public Element Element
        {
            get;
            private set;
        }

        public Exception Error
        {
            get;
            private set;
        }


        public XmppStreamArgs(XmppStreamState state)
        {
            State = state;
        }

        public XmppStreamArgs(XmppStreamState state, Element element, Exception error)
            : this(state)
        {
            Element = element;
            Error = error;
        }
    }
}
