using System;
using System.Net;
using System.Net.Sockets;

namespace Jabber.Net.Server.Connections
{
    class TcpXmppListener : IXmppListener
    {
        private readonly Uri listenUri;
        private TcpListener listener;
        private Action<IXmppConnection> newConnection;


        public TcpXmppListener(Uri listenUri)
        {
            Args.NotNull(listenUri, "listenUri");

            this.listenUri = listenUri;
        }


        public void StartListen(Action<IXmppConnection> newConnection)
        {
            Args.NotNull(newConnection, "newConnection");
            Log.Information("Start listen {0}", listenUri);

            this.newConnection = newConnection;
            var endpoint = new IPEndPoint(IPAddress.Parse(listenUri.Host), listenUri.Port);
            listener = new TcpListener(endpoint)
            {
                ExclusiveAddressUse = true,
            };

            listener.Start();
            BeginAcceptTcpClient(true);
        }

        public void StopListen()
        {
            Log.Information("Stop listen {0}", listenUri);

            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }
        }


        private void OnAccept(IAsyncResult ar)
        {
            var continueAccept = true;
            try
            {
                var listener = (TcpListener)ar.AsyncState;
                var tcpClient = listener.EndAcceptTcpClient(ar);
                newConnection(new TcpXmppConnection(tcpClient));
            }
            catch (ObjectDisposedException)
            {
                //ignore
                continueAccept = false;
            }
            catch (Exception error)
            {
                Log.Error(error);
            }
            finally
            {
                if (continueAccept)
                {
                    BeginAcceptTcpClient(false);
                }
            }
        }

        private void BeginAcceptTcpClient(bool throwerror)
        {
            try
            {
                listener.BeginAcceptTcpClient(OnAccept, listener);
            }
            catch (ObjectDisposedException)
            {
                //ignore
            }
            catch (Exception error)
            {
                if (throwerror) throw;
                else Log.Error(error);
            }
        }
    }
}
