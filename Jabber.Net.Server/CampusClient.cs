using System;
using System.Linq;
using System.Collections.Generic;
using agsXMPP;
using Newtonsoft.Json;

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

        public List<Jid> GetAllUsers(string sessionId)
        {
            var url = BuildUrl("User", "GetAllUsers", new { sessionId });
            var response = Get(url);
            var users = JsonConvert.DeserializeObject<IEnumerable<Campus.Common.User>>(response.Data.ToString()) as IEnumerable<Campus.Common.User>;

            var result = (from o in users
                select new Jid(o.Login, Server, "")).ToList();

            return result;
        }
    }
}

