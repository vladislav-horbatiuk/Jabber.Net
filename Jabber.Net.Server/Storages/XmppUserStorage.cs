using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;
using agsXMPP.util;

namespace Jabber.Net.Server.Storages
{
    public class XmppUserStorage : IXmppUserStorage
    {
        private readonly string connectionStringName;
        private readonly IXmppElementStorage elements;


        public XmppUserStorage(string connectionStringName, IXmppElementStorage elements)
        {
            Args.NotNull(connectionStringName, "connectionStringName");
            Args.NotNull(elements, "elements");

            this.connectionStringName = connectionStringName;
            this.elements = elements;
        }


        public XmppUser GetUser(string username)
        {
            CheckUsername(username);

            return new XmppUser(username, "123");

        }

        public void SaveUser(XmppUser user)
        {
            Args.NotNull(user, "user");
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(user.Name), "User name can not be empty.");
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(user.Password), "User password can not be empty.");


        }

        public bool RemoveUser(string username)
        {
            CheckUsername(username);

            var affected = 0;

            elements.RemoveElements(new Jid(username), "%");
            return 0 < affected;
        }


        public Vcard GetVCard(string username)
        {
            CheckUsername(username);
            return (Vcard)elements.GetElement(new Jid(username), "vcard");
        }

        public void SetVCard(string username, Vcard vcard)
        {
            CheckUsername(username);
            if (vcard == null)
            {
                elements.RemoveElements(new Jid(username), "vcard");
            }
            else
            {
                elements.SaveElement(new Jid(username), "vcard", vcard);
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
