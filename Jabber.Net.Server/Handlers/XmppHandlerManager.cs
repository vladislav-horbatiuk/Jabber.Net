using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.Handlers
{
    public class XmppHandlerManager
    {
        private readonly XmppHandlerRouter router;
        private readonly XmppSessionManager sessionManager;
        private readonly IXmppResolver resolver;
        private readonly XmppHandlerContext context;
        private readonly XmppDefaultHandler defaultHandler;
        private readonly IXmppHandlerInvoker defaultInvoker;


        public IList<Mechanism> SupportedAuthMechanisms
        {
            get;
            private set;
        }

        public bool SupportBind
        {
            get;
            set;
        }

        public bool SupportSession
        {
            get;
            set;
        }

        public bool SupportRegister
        {
            get;
            set;
        }

        public bool SupportTls
        {
            get;
            set;
        }


        public XmppHandlerManager(XmppSessionManager sessionManager, IXmppResolver resolver)
        {
            Args.NotNull(sessionManager, "sessionManager");
            Args.NotNull(resolver, "resolver");

            this.router = new XmppHandlerRouter();
            this.sessionManager = sessionManager;
            this.resolver = resolver;
            this.context = new XmppHandlerContext(this, resolver);

            this.defaultHandler = new XmppDefaultHandler();
            this.defaultInvoker = new XmppHandlerRouter.Invoker<Element>(defaultHandler.ProcessElement, "DefaultHandler");
            this.SupportedAuthMechanisms = new List<Mechanism>();
        }


        public string RegisterHandler(object handler)
        {
            return RegisterHandler(Jid.Empty, handler);
        }

        public string RegisterHandler(Jid jid, object handler)
        {
            if (handler is IXmppRegisterHandler)
            {
                ((IXmppRegisterHandler)handler).OnRegister(context);
            }
            var id = router.RegisterHandler(jid, handler);

            Log.Information("Register handler {0} on {1}, id = {2}", handler.GetType().Name, jid, id);
            return id;
        }

        public void UnregisterHandler(string id)
        {
            Log.Information("Unregister handler {0}", id);
            router.UnregisterHandler(id);
        }


        public void ProcessElement(IXmppConnection connection, Element element)
        {
            //try
            //{
                Args.NotNull(element, "element");

                var session = GetSession(connection);

                if (ProcessValidation(defaultInvoker, element, session, context))
                {
                    var to = element.GetAttribute("to");
                    var jid = to != null && !session.Jid.IsServer ? new Jid(to) : session.Jid;
                    var handlers = router.GetElementHandlers(element.GetType(), jid);
                    var processed = false;

                    foreach (var handler in handlers)
                    {
                        processed = true;
                        if (ProcessValidation(handler, element, session, context))
                        {
                            Log.Information("ProcessElement handler {0} on {1}, session {2}", handler.HandlerId, jid, session.Id);
                            ProcessResult(handler.ProcessElement(element, session, context));
                        }
                        else
                        {
                            Log.Information("ProcessElement skip, handler {0} on {1}, session {2}", handler.HandlerId, jid, session.Id);
                        }
                    }
                    if (!processed)
                    {
                        ProcessResult(defaultInvoker.ProcessElement(element, session, context));
                    }
                }
                else
                {
                    Log.Information("ProcessElement skip, session {0}", session.Id);
                }
            //}
            //catch (Exception error)
            //{
            //    ProcessError(connection, error);
            //}
        }

        public void ProcessClose(IXmppConnection connection)
        {
            try
            {
                var session = GetSession(connection);
                try
                {
                    foreach (var handler in router.GetCloseHandlers())
                    {
                        ProcessResult(handler.OnClose(session, context));
                    }
                }
                finally
                {
                    ProcessResult(defaultHandler.OnClose(session, context));
                }
            }
            catch (Exception error)
            {
                ProcessError(connection, error);
            }
        }

        public void ProcessError(IXmppConnection connection, Exception error)
        {
            try
            {
                Args.NotNull(error, "error");

                var session = GetSession(connection);

                Log.Error("ProcessError session {0}\r\n{1}", session.Id, error);
                try
                {
                    foreach (var handler in router.GetErrorHandlers())
                    {
                        ProcessResult(handler.OnError(error, session, context));
                    }
                }
                finally
                {
                    ProcessResult(defaultHandler.OnError(error, session, context));
                }
            }
            catch (Exception innererror)
            {
                Log.Error(innererror);
            }
        }

        public void ProcessResult(XmppHandlerResult result)
        {
            if (result != null)
            {
                result.Execute(context);
            }
        }

        private bool ProcessValidation(IXmppHandlerInvoker i, Element e, XmppSession s, XmppHandlerContext c)
        {
            foreach (var validator in i.Validators)
            {
                var result = validator.ValidateElement(e, s, c);
                if (result != null)
                {
                    ProcessResult(result);
                    return false;
                }
            }
            return true;
        }


        private XmppSession GetSession(IXmppConnection connection)
        {
            Args.NotNull(connection, "endpoint");
            return sessionManager.GetSession(connection.SessionId) ?? new XmppSession(connection);
        }
    }
}
