using System;
using System.Runtime.Serialization;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server
{
    [Serializable]
    public class JabberStanzaException : JabberException
    {
        private readonly ErrorCondition error;
        private readonly Stanza stanza;
        private readonly string message;


        public override bool CloseStream
        {
            get { return false; }
        }


        public JabberStanzaException(ErrorCondition error, Stanza stanza)
            : this(error, stanza, null)
        {
        }

        public JabberStanzaException(ErrorCondition error, Stanza stanza, string message)
        {
            this.error = error;
            this.stanza = (Stanza)stanza.Clone();
            this.message = message;
        }


        protected JabberStanzaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public override Element ToElement()
        {
            var e = new Error(error);
            if (error == ErrorCondition.InternalServerError)
            {
                e.Message = this.Message;
            }
            if (!string.IsNullOrEmpty(message))
            {
                e.Message = message;
            }
            if (stanza != null)
            {
                var iq = stanza as IQ;
                if (iq != null && iq.Type == IqType.error)
                {
                    return iq;
                }
                if (!stanza.Switched && (iq == null || iq.Type != IqType.result))
                {
                    stanza.SwitchDirection();
                }
                stanza.SetAttribute("type", IqType.error.ToString());
                stanza.ReplaceChild(e);
                return stanza;
            }
            else
            {
                return e;
            }
        }
    }
}
