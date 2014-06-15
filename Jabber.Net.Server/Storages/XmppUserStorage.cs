using agsXMPP;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jabber.Net.Server.Storages
{
    public class XmppUserStorage : IXmppUserStorage
    {
        private readonly string _connectionStringName;
        private readonly IXmppElementStorage _elements;
        const string Server = "jabber.ecampus.kpi.ua";

        private static readonly List<XmppUser> _users = new List<XmppUser>
        {
            new XmppUser("123", "123"),
            new XmppUser("u1", "1"),
            new XmppUser("u2", "2"),
        };

        private static List<RosterItem> _rosterItems;
        private static List<Jid> _subscribers;

        private static List<RosterItem> RosterItems
        {
            get
            {
                if (_rosterItems == null)
                {
                    _rosterItems = Subscribers.Select(o => new RosterItem(o)).ToList();
                }

                return _rosterItems;
            }
        }

        private static IEnumerable<Jid> Subscribers
        {
            get
            {
                if (_subscribers == null)
                {
                    _subscribers = _users.Select(o => new Jid(o.Name, Server, "")).ToList();
                }

                return _subscribers;
            }
        }

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

            return RosterItems;
            return new List<RosterItem>();
        }

        public RosterItem GetRosterItem(Jid user, Jid contact)
        {
            Args.NotNull(user, "user");
            Args.NotNull(contact, "contact");

            var result = (from o in _rosterItems
                          where o.Jid.BareJid == contact.BareJid
                          select o).SingleOrDefault();

            return result;
        }

        public IEnumerable<Jid> GetSubscribers(Jid contact)
        {
            Args.NotNull(contact, "contact");

            //return new List<Jid>();
            return Subscribers;
        }

        public void SaveRosterItem(Jid user, RosterItem ri)
        {
            Args.NotNull(user, "user");
            Args.NotNull(ri, "ri");

            RosterItems.Add(ri);
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
            return Subscribers;
        }

        private void CheckUsername(string username)
        {
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(username), "User name can not be empty.");
        }
    }
}
