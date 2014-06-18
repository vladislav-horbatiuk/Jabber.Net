using System;
using System.IO;
using System.Net;
using System.Text;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Xmpp;

namespace Jabber.Net.Server.Connections
{
    class BoshXmppConnection : IXmppConnection
    {
        private readonly object locker = new object();
        private volatile bool closed = false;
        private readonly HttpListenerContext context;
        private XmppHandlerManager handlerManager;
        private XmppStreamReader reader;


        public string SessionId
        {
            get;
            set;
        }


        public BoshXmppConnection(HttpListenerContext context)
        {
            Args.NotNull(context, "context");
            this.context = context;
        }


        public void BeginReceive(XmppHandlerManager handlerManager)
        {
            RequiresNotClosed();
            Args.NotNull(handlerManager, "handlerManager");
            Log.Information(GetType().Name + " begin receive");

            this.handlerManager = handlerManager;

            reader = new XmppStreamReader(context.Request.InputStream);
            reader.ReadElementComleted += (s, e) =>
            {
                if (e.State == XmppStreamState.Success)
                {
                    Log.Information(GetType().Name + " {0} recv <<:\r\n{1:I}\r\n", SessionId, e.Element);
                    handlerManager.ProcessElement(this, e.Element);
                }
                else if (e.State == XmppStreamState.Error)
                {
                    if (!IgnoreError(e.Error))
                    {
                        Log.Error(e.Error);
                    }
                    Close();
                }
            };
            reader.ReadElementAsync();
        }

        public void Send(Element element, Action<Element> onerror)
        {
            Args.NotNull(element, "element");
            Log.Information(GetType().Name + " {0} send >>:\r\n{1:I}\r\n", SessionId, element);

            try
            {
                var buffer = Encoding.UTF8.GetBytes(element.ToString());
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.ContentType = "text/xml; charset=utf-8";
                context.Response.ContentLength64 = buffer.LongLength;
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Close(buffer, true);
            }
            catch (Exception ex)
            {
                if (!IgnoreError(ex))
                {
                    Log.Error(ex);
                }
                if (onerror != null)
                {
                    onerror(element);
                }
            }
            finally
            {
                Close();
            }
        }

        public void Reset()
        {
            Log.Information(GetType().Name + " {0} reset", SessionId);
        }

        public void Close()
        {
            lock (locker)
            {
                if (closed) return;
                closed = true;

                Log.Information(GetType().Name + " {0} close", SessionId);

                try
                {
                    context.Request.InputStream.Close();
                }
                catch (Exception) { }
                try
                {
                    context.Response.Close();
                }
                catch (Exception) { }
            }
        }

        private void RequiresNotClosed()
        {
            Args.Requires<ObjectDisposedException>(!closed, GetType().FullName);
        }

        private bool IgnoreError(Exception error)
        {
            return error is ObjectDisposedException || error is IOException;
        }
    }
}
