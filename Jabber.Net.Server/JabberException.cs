using System;
using System.Runtime.Serialization;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server
{
    [Serializable]
    public abstract class JabberException : Exception
    {
        public abstract bool CloseStream
        {
            get;
        }


        public JabberException()
            : base()
        {
        }

        public JabberException(string message)
            : base(message)
        {
        }

        public JabberException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected JabberException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public abstract Element ToElement();
    }
}
