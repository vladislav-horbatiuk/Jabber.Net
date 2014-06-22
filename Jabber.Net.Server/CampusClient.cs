using agsXMPP;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jabber.Net.Server
{
    public class CampusClient : Campus.SDK.Client
    {
        private const string Server = "jabber.ecampus.kpi.ua";

        public XmppUser GetUser(string username)
        {
            var url = BuildUrl("User", "JabberAuth", new { username, applicationKey = "applicationKey" });
            var response = Get(url);
            var password = response.Data.ToString();

            if (String.IsNullOrEmpty(password))
            {
                return null;
            }

            return new XmppUser(username, password);
        }

        public IEnumerable<Jid> GetAllUsers(string sessionId)
        {
            var url = BuildUrl("User", "GetAllUsers", new { sessionId });
            var response = Get(url);
            var users = JsonConvert.DeserializeObject<IEnumerable<Campus.Common.User>>(response.Data.ToString()) as IEnumerable<Campus.Common.User>;

            var result = (from o in users
                          select new Jid(o.Login, Server, "")).ToList();

            return result;
        }

        public Vcard GetVCard(string username)
        {
            throw new NotImplementedException();
        }

        public RosterItem GetRosterItem(Jid contact)
        {
            return new RosterItem(contact);
        }

        public IEnumerable<RosterItem> GetRosterItems(Jid contact)
        {
            XmppUser user = GetUser(contact.User);
            string sessionId = Authenticate(user.Name, user.Password);
            return GetAllUsers(sessionId)
                .Select(jid =>
                    new RosterItem { Jid = jid, Name = jid.User, Subscription = SubscriptionType.both });

        }
    }
}

