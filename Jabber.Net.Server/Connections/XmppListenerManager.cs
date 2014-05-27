using System;
using System.Collections.Generic;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Connections
{
    public class XmppListenerManager
    {
        private readonly IList<IXmppListener> listeners = new List<IXmppListener>();
        private readonly XmppHandlerManager handlerManager;
        private bool listen = false;


        public XmppListenerManager(XmppHandlerManager handlerManager)
        {
            Args.NotNull(handlerManager, "handlerManager");

            this.handlerManager = handlerManager;
        }


        public void AddListener(IXmppListener listener)
        {
            Args.NotNull(listener, "listener");
            RequiresNotListen();

            listeners.Add(listener);
        }

        public void RemoveListener(IXmppListener listener)
        {
            Args.NotNull(listener, "listener");
            RequiresNotListen();

            listeners.Remove(listener);
        }


        public void StartListen()
        {
            RequiresNotListen();

            foreach (var listener in listeners)
            {
                try
                {
                    listener.StartListen(NewConnection);
                }
                catch (Exception error)
                {
                    Log.Error(error);
                }
            }
            listen = true;
        }

        public void StopListen()
        {
            if (listen)
            {
                foreach (var listener in listeners)
                {
                    try
                    {
                        listener.StopListen();
                    }
                    catch (Exception error)
                    {
                        Log.Error(error);
                    }
                }
                listen = false;
            }
        }


        private void NewConnection(IXmppConnection connection)
        {
            try
            {
                connection.BeginReceive(handlerManager);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }


        private void RequiresNotListen()
        {
            Args.Requires<InvalidOperationException>(!listen, "ListenerManager in listen state.");
        }
    }
}
