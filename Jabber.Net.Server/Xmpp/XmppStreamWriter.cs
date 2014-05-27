using System;
using System.IO;
using System.Text;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Xmpp
{
    class XmppStreamWriter
    {
        private readonly Stream stream;
        private readonly Encoding encoding;
        private volatile bool sending;


        public event EventHandler<XmppStreamArgs> WriteElementComleted;


        public XmppStreamWriter(Stream stream)
        {
            Args.NotNull(stream, "stream");

            this.stream = stream;
            this.encoding = Encoding.UTF8;
        }


        public void WriteElementAsync(Element element, Action<Element> onerror)
        {
            WriteElementCancel();
            try
            {
                sending = true;
                var buffer = encoding.GetBytes(element.ToString());
                stream.BeginWrite(buffer, 0, buffer.Length, SendCallback, Tuple.Create(element, onerror));
            }
            catch (Exception ex)
            {
                sending = false;
                OnError(element, ex, onerror);
            }
        }

        public void WriteElementCancel()
        {
            while (sending) ;
        }


        private void SendCallback(IAsyncResult ar)
        {
            var data = (Tuple<Element, Action<Element>>)ar.AsyncState;
            var element = data.Item1;
            var onerror = data.Item2;
            try
            {
                try
                {
                    stream.EndWrite(ar);
                    stream.Flush();
                }
                finally
                {
                    sending = false;
                }

                var ev = WriteElementComleted;
                if (ev != null)
                {
                    ev(this, new XmppStreamArgs(XmppStreamState.Success, element, null));
                }
            }
            catch (Exception ex)
            {
                OnError(element, ex, onerror);
            }
        }

        private void OnError(Element element, Exception exception, Action<Element> onerror)
        {
            try
            {
                if (onerror != null)
                {
                    onerror(element);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            var ev = WriteElementComleted;
            if (ev != null)
            {
                ev(this, new XmppStreamArgs(XmppStreamState.Error, element, exception));
            }
        }
    }
}
