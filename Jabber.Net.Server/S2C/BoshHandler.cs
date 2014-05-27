using System;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.bosh;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class BoshHandler : XmppHandler, IXmppHandler<Body>
    {
        public TimeSpan WaitTimeout
        {
            get;
            set;
        }

        public TimeSpan InactivityTimeout
        {
            get;
            set;
        }

        public TimeSpan SendTimeout
        {
            get;
            set;
        }

        public int Hold
        {
            get;
            set;
        }

        public int Window
        {
            get;
            set;
        }


        public BoshHandler()
        {
            WaitTimeout = TimeSpan.FromMinutes(1);
            InactivityTimeout = TimeSpan.FromMinutes(2);
            SendTimeout = TimeSpan.FromSeconds(5);
            Hold = 1;
            Window = 5;
        }


        public XmppHandlerResult ProcessElement(Body element, XmppSession session, XmppHandlerContext context)
        {
            XmppSession boshSession;
            BoshXmppAggregator aggregator;

            if (string.IsNullOrEmpty(element.Sid))
            {
                if (element.Hold == 0 || element.Wait == 0)
                {
                    element.Hold = 1;
                }
                else if (Hold < element.Hold)
                {
                    element.Hold = Hold;
                }
                element.Requests = element.Hold + 1;
                if (element.Window == 0 || Window < element.Window)
                {
                    element.Window = Window;
                }
                if (element.Wait == 0 || WaitTimeout.TotalSeconds < element.Wait)
                {
                    element.Wait = (int)WaitTimeout.TotalSeconds;
                }
                if (element.Inactivity == 0 || InactivityTimeout.TotalSeconds < element.Inactivity)
                {
                    element.Inactivity = (int)InactivityTimeout.TotalSeconds;
                }

                aggregator = new BoshXmppAggregator(
                        session.Id,
                        TimeSpan.FromSeconds(element.Wait),
                        TimeSpan.FromSeconds(element.Inactivity),
                        SendTimeout,
                        element.Hold,
                        element.Window);
                aggregator.BeginReceive(context.Handlers);
                boshSession = new XmppSession(aggregator);
            }
            else
            {
                boshSession = context.Sessions.GetSession(element.Sid);
                if (boshSession != null)
                {
                    aggregator = (BoshXmppAggregator)boshSession.Connection;
                }
                else
                {
                    return Error(session, agsXMPP.protocol.StreamErrorCondition.ImproperAddressing);
                }
            }
            aggregator.AddConnection(element.Rid, session.Connection);

            if (element.Type == BoshType.terminate)
            {
                return Close(boshSession);
            }
            else if (string.IsNullOrEmpty(element.Sid))
            {
                return StartBoshSession(element, boshSession, context);
            }
            else if (element.XmppRestart)
            {
                return RestartBoshSession(element, boshSession, context);
            }

            return Void();
        }


        private XmppHandlerResult StartBoshSession(Body element, XmppSession session, XmppHandlerContext context)
        {
            var body = new Body
            {
                XmppVersion = "1.0",
                Sid = session.Id,
                From = element.To,
                Secure = false,
                Inactivity = element.Inactivity,
                Wait = element.Wait,
                Hold = element.Hold,
                Window = element.Window,
                Requests = element.Requests,
            };
            body.SetAttribute("xmlns:xmpp", "urn:xmpp:xbosh");
            body.SetAttribute("xmpp:restartlogic", true);

            var stream = new Stream
            {
                Prefix = agsXMPP.Uri.PREFIX,
                DefaultNamespace = agsXMPP.Uri.CLIENT,
                Version = element.XmppVersion,
                To = element.To,
                Language = element.GetAttribute("xml:lang"),
            };

            return Component(Send(session, body), Process(session, stream), Send(session, new BodyEnd()));
        }

        private XmppHandlerResult RestartBoshSession(Body element, XmppSession session, XmppHandlerContext context)
        {
            var stream = new Stream
            {
                Prefix = agsXMPP.Uri.PREFIX,
                DefaultNamespace = agsXMPP.Uri.CLIENT,
                To = element.To,
                From = element.From,
            };

            return Process(session, stream);
        }
    }
}
