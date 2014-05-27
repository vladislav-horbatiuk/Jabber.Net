using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using agsXMPP;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Collections;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers
{
    class XmppHandlerRouter
    {
        private readonly static IUniqueId uniqueId = new IncrementalUniqueId();
        private readonly MethodInfo registerHandlerInternal;
        private readonly XmppHandlerStorage invokers = new XmppHandlerStorage();
        private readonly IDictionary<string, IXmppCloseHandler> closers = new ReaderWriterLockDictionary<string, IXmppCloseHandler>(50);
        private readonly IDictionary<string, IXmppErrorHandler> errors = new ReaderWriterLockDictionary<string, IXmppErrorHandler>(50);


        public XmppHandlerRouter()
        {
            registerHandlerInternal = GetType().GetMethod("RegisterHandlerInternal", BindingFlags.Instance | BindingFlags.NonPublic);
        }


        public string RegisterHandler(Jid jid, object handler)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(handler, "handler");

            var id = uniqueId.CreateId();
            foreach (var m in handler.GetType().GetMethods())
            {
                var parameters = m.GetParameters();
                if (parameters.Length == 3 &&
                    parameters[0].ParameterType.IsSubclassOf(typeof(Element)) &&
                    parameters[1].ParameterType == typeof(XmppSession) &&
                    parameters[2].ParameterType == typeof(XmppHandlerContext))
                {
                    var registerHandlerGeneric = registerHandlerInternal.MakeGenericMethod(parameters[0].ParameterType);
                    var callback = Delegate.CreateDelegate(registerHandlerGeneric.GetParameters()[1].ParameterType, handler, m);
                    registerHandlerGeneric.Invoke(this, new object[] { jid, callback, id });
                }
            }

            RegisterHandler(id, handler);

            return id;
        }

        public void UnregisterHandler(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                invokers.Unregister(id);
                closers.Remove(id);
                errors.Remove(id);
            }
        }

        public IEnumerable<IXmppHandlerInvoker> GetElementHandlers(Type t, Jid j)
        {
            Args.NotNull(j, "j");
            Args.NotNull(t, "t");

            return invokers.GetInvokers(t, j);
        }

        public IEnumerable<IXmppCloseHandler> GetCloseHandlers()
        {
            return closers.Values;
        }

        public IEnumerable<IXmppErrorHandler> GetErrorHandlers()
        {
            return errors.Values;
        }


        private void RegisterHandlerInternal<T>(Jid jid, Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler, string id) where T : Element
        {
            invokers.Register(typeof(T), jid, new Invoker<T>(handler, id));
        }

        private void RegisterHandler(string id, object handler)
        {
            if (handler is IXmppCloseHandler)
            {
                closers[id] = (IXmppCloseHandler)handler;
            }
            if (handler is IXmppErrorHandler)
            {
                errors[id] = (IXmppErrorHandler)handler;
            }
        }


        internal class Invoker<T> : IXmppHandlerInvoker where T : Element
        {
            private readonly Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler;

            public string HandlerId { get; private set; }

            public IEnumerable<XmppValidationAttribute> Validators { get; private set; }


            public Invoker(Func<T, XmppSession, XmppHandlerContext, XmppHandlerResult> handler, string handlerId)
            {
                this.handler = handler;
                HandlerId = handlerId;
                Validators = handler.Method.GetCustomAttributes(false).OfType<XmppValidationAttribute>().ToArray();
            }

            public XmppHandlerResult ProcessElement(Element e, XmppSession s, XmppHandlerContext c)
            {
                return handler((T)e, s, c);
            }
        }
    }
}
