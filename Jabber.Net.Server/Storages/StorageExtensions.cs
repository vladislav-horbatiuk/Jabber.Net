using System;
using System.Collections.Generic;
using agsXMPP;
using agsXMPP.Xml.Dom;
using agsXMPP.protocol.client;
using agsXMPP.protocol.x;

namespace Jabber.Net.Server.Storages
{
    public static class StorageExtensions
    {
        public static void SaveOffline(this IXmppElementStorage storage, Jid jid, Element element)
        {
            var id = element.GetAttribute("id");
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString("N");
            }

            var message = element as Message;
            if (message != null)
            {
                message.XDelay = new Delay { Stamp = DateTime.UtcNow, };
            }

            storage.SaveElement(jid, "offline|" + id, element);
        }

        public static IEnumerable<Element> GetOfflines(this IXmppElementStorage storage, Jid jid)
        {
            return storage.GetElements(jid, "offline|%");
        }

        public static void RemoveOfflines(this IXmppElementStorage storage, Jid jid)
        {
            storage.RemoveElements(jid, "offline|%");
        }
    }
}
