using agsXMPP;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;
using System.Collections.Generic;

namespace Jabber.Net.Server.Storages
{
    public interface IXmppUserStorage
    {
        XmppUser GetUser(string username);
        
        void SaveUser(XmppUser user);

        bool RemoveUser(string username);

        Vcard GetVCard(string username);

        void SetVCard(string username, Vcard vcard);

        IEnumerable<RosterItem> GetRosterItems(Jid user);

        IEnumerable<Jid> GetAskers(Jid contact);

        IEnumerable<Jid> GetSubscribers(Jid contact);

        RosterItem GetRosterItem(Jid user, Jid contact);

        void SaveRosterItem(Jid user, RosterItem ri);

        bool RemoveRosterItem(Jid user, Jid contact);

        
    }
}
