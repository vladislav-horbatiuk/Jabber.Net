using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.iq.privacy;
using agsXMPP.util;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Storages
{
    public class XmppElementStorage : IXmppElementStorage
    {
        public XmppElementStorage(string connectionStringName)
        {
            Args.NotNull(connectionStringName, "connectionStringName");
        }


        public IEnumerable<Element> GetElements(Jid jid, string key)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(key, "key");

            return new List<Element>();
        }

        public Element GetElement(Jid jid, string key)
        {
            return GetElements(jid, key).FirstOrDefault();
        }

        public void SaveElement(Jid jid, string key, Element element)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(key, "key");
            Args.NotNull(element, "element");

        }


        public bool RemoveElements(Jid jid, string key)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(key, "key");

            return true;
        }


    }
}
