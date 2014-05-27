using System;
using System.Runtime.Serialization;
using agsXMPP.protocol;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server
{
    [Serializable]
    public class JabberStreamException : JabberException
    {
        private readonly StreamErrorCondition error;


        public override bool CloseStream
        {
            get { return true; }
        }


        public JabberStreamException(StreamErrorCondition error)
        {
            this.error = error;
        }

        public JabberStreamException(Exception exception)
            : base(exception.Message, exception)
        {
            this.error = StreamErrorCondition.InternalServerError;
        }

        protected JabberStreamException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public override Element ToElement()
        {
            var e = new Error(error) { Prefix = agsXMPP.Uri.PREFIX };
            if (error == StreamErrorCondition.InternalServerError)
            {
                e.Text = Message;
            }
            return e;
        }
    }
}
