using System.Collections.Generic;
using agsXMPP;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Storages
{
    public interface IXmppElementStorage
    {
        IEnumerable<Element> GetElements(Jid jid, string key);

        Element GetElement(Jid jid, string key);

        void SaveElement(Jid jid, string key, Element element);

        bool RemoveElements(Jid jid, string key);
    }
}
