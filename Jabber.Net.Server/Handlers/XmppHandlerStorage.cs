using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using agsXMPP;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Handlers
{
    class XmppHandlerStorage
    {
        private readonly ReaderWriterLock locker = new ReaderWriterLock();
        private readonly IUniqueId uniqueId = new GuidUniqueId();
        private readonly Dictionary<string, IXmppHandlerInvoker> handlers = new Dictionary<string, IXmppHandlerInvoker>();
        private readonly RouterStore<string> handlerIds = new RouterStore<string>();
        private readonly RouterStore<Type> types = new RouterStore<Type>();
        private readonly RouterStore<string> jids = new RouterStore<string>();


        public void Register(Type type, Jid jid, IXmppHandlerInvoker invoker)
        {
            using (locker.WriteLock())
            {
                var id = uniqueId.CreateId();
                types.Add(type, id);
                jids.Add(jid.ToString(), id);
                handlerIds.Add(invoker.HandlerId, id);
                handlers.Add(id, invoker);
            }
        }

        public void Unregister(string handlerId)
        {
            using (locker.WriteLock())
            {
                foreach (var id in handlerIds.GetIdentifiers(handlerId))
                {
                    types.Remove(id);
                    jids.Remove(id);
                    handlers.Remove(id);
                }
                handlerIds.Remove(handlerId);
            }
        }

        public IEnumerable<IXmppHandlerInvoker> GetInvokers(Type type, Jid jid)
        {
            using (locker.ReadLock())
            {
                var byType = Enumerable.Empty<string>();
                var byJid = Enumerable.Empty<string>();

                while (type != null && type != typeof(object))
                {
                    byType = byType.Union(types.GetIdentifiers(type));
                    type = type.BaseType;
                }
                foreach (var pattern in jids.Keys)
                {
                    if (Regex.IsMatch(jid.ToString(), pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline))
                    {
                        byJid = byJid.Union(jids.GetIdentifiers(pattern));
                    }
                }
                return byType.Reverse()
                    .Intersect(byJid)
                    .Select(id => handlers[id])
                    .ToArray();
            }
        }


        private class RouterStore<T>
        {
            private readonly Dictionary<T, List<string>> store = new Dictionary<T, List<string>>(100);


            public IEnumerable<T> Keys
            {
                get { return store.Keys; }
            }


            public void Add(T value, string id)
            {
                if (!store.ContainsKey(value))
                {
                    store.Add(value, new List<string>());
                }
                store[value].Add(id);
            }

            public void Remove(string id)
            {
                foreach (var list in store.Values)
                {
                    list.RemoveAll(item => item == id);
                }
                foreach (var pair in new Dictionary<T, List<string>>(store))
                {
                    if (pair.Value.Count == 0)
                    {
                        store.Remove(pair.Key);
                    }
                }
            }

            public IEnumerable<string> GetIdentifiers(T value)
            {
                List<string> list;
                return store.TryGetValue(value, out list) ? list : Enumerable.Empty<string>();
            }
        }
    }
}