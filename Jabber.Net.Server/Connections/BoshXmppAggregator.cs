using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP.protocol;
using agsXMPP.protocol.extensions.bosh;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Connections
{
    class BoshXmppAggregator : IXmppConnection
    {
        private readonly Dictionary<long, IXmppConnection> connections = new Dictionary<long, IXmppConnection>();
        private XmppHandlerManager handlerManager;

        private readonly TimeSpan waitTimeout;
        private readonly TimeSpan inactivityTimeout;
        private readonly TimeSpan sendTimeout;
        private readonly int hold;
        private readonly int window;

        private readonly List<Tuple<Element, Action<Element>>> buffer = new List<Tuple<Element, Action<Element>>>();


        public string SessionId
        {
            get;
            set;
        }


        public BoshXmppAggregator(string sessionId, TimeSpan waitTimeout, TimeSpan inactivityTimeout, TimeSpan sendTimeout, int hold, int window)
        {
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(sessionId), "Argument sessionId can not by empty.");

            this.SessionId = sessionId;
            this.waitTimeout = waitTimeout;
            this.inactivityTimeout = inactivityTimeout;
            this.sendTimeout = sendTimeout;
            this.hold = hold;
            this.window = window;
        }


        public IXmppConnection AddConnection(long rid, IXmppConnection connection)
        {
            Args.NotNull(connection, "connection");

            lock (connections)
            {
                if (hold == connections.Count)
                {
                    throw new JabberStreamException(StreamErrorCondition.PolicyViolation);
                }
                foreach (var pair in connections)
                {
                    if (window < Math.Abs(rid - pair.Key))
                    {
                        throw new JabberStreamException(StreamErrorCondition.PolicyViolation);
                    }
                }

                // cancel inactivity
                TaskQueue.RemoveTask(SessionId);

                connection.SessionId = SessionId;
                connections.Add(rid, connection);
                // add wait timeout
                TaskQueue.AddTask(rid.ToString(), () => SendAndClose(rid, new Body(), null), waitTimeout);
            }
            return this;
        }

        public void BeginReceive(XmppHandlerManager handlerManager)
        {
            Args.NotNull(handlerManager, "handlerManager");

            this.handlerManager = handlerManager;
        }

        public void Send(Element element, Action<Element> onerror)
        {
            // Send a single item or to accumulate a buffer until a bodyend.
            Body body = null;
            Action<Element> commonOnError = null;

            lock (buffer)
            {
                if (!(element is BodyEnd))
                {
                    element = PrepareElement(element);
                    buffer.Add(Tuple.Create(element, onerror));

                    if (buffer.Any(t => t.Item1 is Body))
                    {
                        return;
                    }
                }

                var elements = buffer.Select(t => t.Item1);
                body = elements.FirstOrDefault(e => e is Body) as Body ?? new Body();
                foreach (var e in elements.Where(e => !(e is Body)))
                {
                    body.AddChild(e);
                    if (e is agsXMPP.protocol.Error || e is agsXMPP.protocol.sasl.Failure || e is agsXMPP.protocol.tls.Failure)
                    {
                        body.Type = BoshType.terminate;
                        body.SetAttribute("condition", "remote-stream-error");
                    }
                }

                var onerrors = (from t in buffer where t.Item2 != null select new Action(() => t.Item2(t.Item1))).ToList();
                commonOnError = _ => onerrors.ForEach(e => e());

                buffer.Clear();
            }
            SendBody(body, commonOnError);
        }

        public void Reset()
        {

        }

        public void Close()
        {
            handlerManager.ProcessClose(this);

            IEnumerable<long> copy = null;
            lock (connections)
            {
                copy = connections.Keys.ToArray();
            }

            Action closeall = () =>
            {
                foreach (var rid in copy)
                {
                    SendAndClose(rid, new Body { Type = BoshType.terminate }, null);
                    // cancel inactivity
                    TaskQueue.RemoveTask(SessionId);
                }
            };
            closeall.BeginInvoke(null, null);
        }


        private void SendBody(Body body, Action<Element> onerror)
        {
            var timeout = TimeSpan.Zero;
            lock (connections)
            {
                if (connections.Count == 0)
                {
                    timeout = sendTimeout;
                }
            }

            Action send = () =>
            {
                var minrid = 0L;
                lock (connections)
                {
                    minrid = connections.Any() ? connections.Min(p => p.Key) : 0;
                }
                // cancel wait timeout
                TaskQueue.RemoveTask(minrid.ToString());

                SendAndClose(minrid, body, onerror);
            };
            TaskQueue.AddTask(Guid.NewGuid().ToString(), send, timeout);
        }

        private void SendAndClose(long rid, Body body, Action<Element> onerror)
        {
            try
            {
                IXmppConnection c;
                lock (connections)
                {
                    if (connections.TryGetValue(rid, out c))
                    {
                        connections.Remove(rid);
                    }
                    if (connections.Count == 0)
                    {
                        // set inactivity callback
                        TaskQueue.RemoveTask(SessionId);
                        TaskQueue.AddTask(SessionId, () => Close(), inactivityTimeout);
                    }
                }
                if (c != null)
                {
                    c.Send(body, onerror);
                }
                else if (onerror != null)
                {
                    onerror(body);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                if (onerror != null)
                {
                    onerror(body);
                }
            }
        }

        private Element PrepareElement(Element e)
        {
            var stream = e as agsXMPP.protocol.client.Stream;
            if (stream != null)
            {
                return stream.Features;
            }

            return e;
        }
    }
}
