using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Xmpp;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppConnection : IXmppConnection, IXmppTlsConnection
    {
        private readonly object _locker = new object();
        private volatile bool _closed = false;

        private readonly TcpClient _client;
        private Stream _stream;

        private XmppHandlerManager _handlerManager;
        private XmppStreamReader _reader;
        private XmppStreamWriter _writer;

        public string SessionId
        {
            get;
            set;
        }

        public bool TlsStarted
        {
            get
            {
                return false;
                return _stream is SslStream;
            }
        }


        public TcpXmppConnection(TcpClient tcpClient)
        {
            Args.NotNull(tcpClient, "tcpClient");

            _client = tcpClient;
            _stream = _client.GetStream();
        }


        public void BeginReceive(XmppHandlerManager handlerManager)
        {
            RequiresNotClosed();
            Args.NotNull(handlerManager, "handlerManager");
            Log.Information(GetType().Name + " begin receive");

            this._handlerManager = handlerManager;
            Reset();
        }


        public void Reset()
        {
            Log.Information(GetType().Name + " {0} reset", SessionId);

            if (_reader != null)
            {
                _reader.ReadElementCancel();
            }

            _reader = new XmppStreamReader(_stream);
            _reader.ReadElementComleted += (s, e) =>
            {
                if (e.State == XmppStreamState.Success)
                {
                    Log.Information(GetType().Name + " {0} recv <<:\r\n{1:I}\r\n", SessionId, e.Element);
                    _handlerManager.ProcessElement(this, e.Element);
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
            _reader.ReadElementAsync();

            if (_writer != null)
            {
                _writer.WriteElementCancel();
            }
            _writer = new XmppStreamWriter(_stream);
            _writer.WriteElementComleted += (s, e) =>
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

            _writer.WriteElementAsync(element, onerror);
        }

        public void Close()
        {
            lock (_locker)
            {
                if (_closed) return;
                _closed = true;

                Log.Information(GetType().Name + " {0} close", SessionId);

                try
                {
                    if (_stream != null)
                    {
                        _stream.Close();
                    }
                }
                catch (Exception) { }
                try
                {
                    if (_client != null)
                    {
                        _client.Close();
                    }
                }
                catch (Exception) { }
                try
                {
                    if (_handlerManager != null)
                    {
                        _handlerManager.ProcessClose(this);
                    }
                }
                catch (Exception) { }
            }
        }

        public void TlsStart(X509Certificate certificate)
        {
            Args.NotNull(certificate, "certificate");
            Log.Information(GetType().Name + " {0} start tls", SessionId);

            _stream.Flush();
            if (_reader != null)
            {
                _reader.ReadElementCancel();
            }
            
            //_stream = new SslStream(_stream);
            //((SslStream)_stream).AuthenticateAsServer(certificate, false, SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Ssl2, true);

            Reset();
        }


        private void RequiresNotClosed()
        {
            Args.Requires<ObjectDisposedException>(!_closed, GetType().FullName);
        }

        private bool IgnoreError(Exception error)
        {
            return error is ObjectDisposedException || error is IOException;
        }
    }
}
