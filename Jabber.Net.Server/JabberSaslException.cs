using System;
using System.Runtime.Serialization;
using agsXMPP.protocol;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server
{
    [Serializable]
    public class JabberSaslException : JabberStreamException
    {
        private readonly FailureCondition error;


        public JabberSaslException(FailureCondition error)
            : base(StreamErrorCondition.NotAuthorized)
        {
            this.error = error;
        }


        protected JabberSaslException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public override Element ToElement()
        {
            return new Failure(error);
        }
    }
}
