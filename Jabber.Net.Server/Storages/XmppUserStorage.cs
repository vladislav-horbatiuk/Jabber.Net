using System.Linq;
using agsXMPP;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;
using System;
using System.Collections.Generic;

namespace Jabber.Net.Server.Storages
{
    public class XmppUserStorage : IXmppUserStorage
    {
        private readonly string _connectionStringName;
        private readonly IXmppElementStorage _elements;

        private static readonly List<XmppUser> _users = new List<XmppUser>
        {
            new XmppUser("123", "123"),
            new XmppUser("1", "1"),
        };

        private static readonly List<RosterItem> _rosterItems = new List<RosterItem>
        {
            new RosterItem(new Jid("1")),
            new RosterItem(new Jid("2")),
        };
        
        public XmppUserStorage(string connectionStringName, IXmppElementStorage elements)
        {
            _connectionStringName = connectionStringName;
            _elements = elements;
        }
        
        public XmppUser GetUser(string username)
        {
            CheckUsername(username);

            return _users.FirstOrDefault(o => o.Name == username);
        }

        public void SaveUser(XmppUser user)
        {
            Args.NotNull(user, "user");
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(user.Name), "User name can not be empty.");
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(user.Password), "User password can not be empty.");

            _users.Add(user);
        }

        public bool RemoveUser(string username)
        {
            CheckUsername(username);

            var user = GetUser(username);

            if (user != null)
            {
                _users.Remove(user);
                _elements.RemoveElements(new Jid(username), "%");
                return true;
            }

            return false;
        }

        public Vcard GetVCard(string username)
        {
            CheckUsername(username);
            return (Vcard)_elements.GetElement(new Jid(username), "vcard");
        }

        public void SetVCard(string username, Vcard vcard)
        {
            CheckUsername(username);

            if (vcard == null)
            {
                _elements.RemoveElements(new Jid(username), "vcard");
            }
            else
            {
                _elements.SaveElement(new Jid(username), "vcard", vcard);
            }
        }

        public IEnumerable<RosterItem> GetRosterItems(Jid user)
        {
            Args.NotNull(user, "user");

            return new List<RosterItem>();
        }

        public RosterItem GetRosterItem(Jid user, Jid contact)
        {
            Args.NotNull(user, "user");
            Args.NotNull(contact, "contact");

            return new RosterItem();
        }

        public IEnumerable<Jid> GetSubscribers(Jid contact)
        {
            Args.NotNull(contact, "contact");
            return new List<Jid>();

        }

        public void SaveRosterItem(Jid user, RosterItem ri)
        {
            Args.NotNull(user, "user");
            Args.NotNull(ri, "ri");

            _rosterItems.Add(ri);

        }

        public bool RemoveRosterItem(Jid user, Jid contact)
        {
            Args.NotNull(user, "user");
            Args.NotNull(contact, "contact");

            return true;

        }

        public IEnumerable<Jid> GetAskers(Jid contact)
        {
            Args.NotNull(contact, "contact");

            return new List<Jid>();
        }
        
        private void CheckUsername(string username)
        {
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(username), "User name can not be empty.");
        }
    }
}
