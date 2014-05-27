using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Xmpp;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppConnection : IXmppConnection, IXmppTlsConnection
    {
        private readonly object locker = new object();
        private volatile bool closed = false;

        private readonly TcpClient client;
        private Stream stream;

        private XmppHandlerManager handlerManager;
        private XmppStreamReader reader;
        private XmppStreamWriter writer;

        public string SessionId
        {
            get;
            set;
        }

        public bool TlsStarted
        {
            get { return stream is SslStream; }
        }


        public TcpXmppConnection(TcpClient tcpClient)
        {
            Args.NotNull(tcpClient, "tcpClient");

            client = tcpClient;
            stream = client.GetStream();
        }


        public void BeginReceive(XmppHandlerManager handlerManager)
        {
            RequiresNotClosed();
            Args.NotNull(handlerManager, "handlerManager");
            Log.Information(GetType().Name + " begin receive");

            this.handlerManager = handlerManager;
            Reset();
        }


        public void Reset()
        {
            Log.Information(GetType().Name + " {0} reset", SessionId);

            if (reader != null)
            {
                reader.ReadElementCancel();
            }

            reader = new XmppStreamReader(stream);
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
                else if (e.State == XmppStreamState.Closed)
                {
                    Close();
                }
            };
            reader.ReadElementAsync();

            if (writer != null)
            {
                writer.WriteElementCancel();
            }
            writer = new XmppStreamWriter(stream);
            writer.WriteElementComleted += (s, e) =>
            {
                if (e.State == XmppStreamState.Error)
                {
                    if (!IgnoreError(e.Error))
                    {
                        Log.Error(e.Error);
                    }
                    Close();
                }
            };
        }

        public void Send(Element element, Action<Element> onerror)
        {
            Args.NotNull(element, "element");
            Log.Information(GetType().Name + " {0} send >>:\r\n{1:I}\r\n", SessionId, element);

            writer.WriteElementAsync(element, onerror);
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
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                catch (Exception) { }
                try
                {
                    if (client != null)
                    {
                        client.Close();
                    }
                }
                catch (Exception) { }
                try
                {
                    if (handlerManager != null)
                    {
                        handlerManager.ProcessClose(this);
                    }
                }
                catch (Exception) { }
            }
        }

        public void TlsStart(X509Certificate certificate)
        {
            Args.NotNull(certificate, "certificate");
            Log.Information(GetType().Name + " {0} start tls", SessionId);

            stream.Flush();
            if (reader != null)
            {
                reader.ReadElementCancel();
            }

            stream = new SslStream(stream);
            ((SslStream)stream).AuthenticateAsServer(certificate, false, SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Ssl2, true);

            Reset();
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
