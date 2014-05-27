using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.vcard;
using agsXMPP.util;
using Jabber.Net.Server.Data;
using Jabber.Net.Server.Data.Sql;

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

            CreateSchema();
        }


        public XmppUser GetUser(string username)
        {
            CheckUsername(username);
            using (var db = GetDb())
            {
                return db
                    .ExecList(new SqlQuery("jabber_user").Select("username", "userpass").Where("username", username))
                    .Select(r => new XmppUser((string)r[0], (string)r[1]))
                    .SingleOrDefault();
            }
        }

        public void SaveUser(XmppUser user)
        {
            Args.NotNull(user, "user");
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(user.Name), "User name can not be empty.");
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(user.Password), "User password can not be empty.");

            using (var db = GetDb())
            using (var tx = db.BeginTransaction())
            {
                var exists = 0 < db.ExecScalar<int>(new SqlQuery("jabber_user").SelectCount().Where("username", user.Name));
                var q = exists ?
                    (ISqlInstruction)new SqlUpdate("jabber_user").Set("userpass", user.Password).Where("username", user.Name) :
                    (ISqlInstruction)new SqlInsert("jabber_user").InColumnValue("username", user.Name).InColumnValue("userpass", user.Password);
                db.ExecuteNonQuery(q);
                tx.Commit();
            }
        }

        public bool RemoveUser(string username)
        {
            CheckUsername(username);

            var affected = 0;
            using (var db = GetDb())
            {
                affected = db.ExecuteNonQuery(new SqlDelete("jabber_user").Where("username", username));
            }
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
            using (var db = GetDb())
            {
                return db.ExecList(new SqlQuery("jabber_roster").Select("item").Where("user_jid", user.Bare))
                    .Select(r => ElementSerializer.DeSerializeElement<RosterItem>((string)r[0]))
                    .ToArray();
            }
        }

        public RosterItem GetRosterItem(Jid user, Jid contact)
        {
            Args.NotNull(user, "user");
            Args.NotNull(contact, "contact");
            using (var db = GetDb())
            {
                var s = db.ExecScalar<string>(new SqlQuery("jabber_roster").Select("item").Where("user_jid", user.Bare).Where("contact_jid", contact.Bare));
                return !string.IsNullOrEmpty(s) ? ElementSerializer.DeSerializeElement<RosterItem>(s) : null;
            }
        }

        public IEnumerable<Jid> GetSubscribers(Jid contact)
        {
            Args.NotNull(contact, "contact");
            using (var db = GetDb())
            {
                var q = new SqlQuery("jabber_roster")
                    .Select("user_jid")
                    .Where("contact_jid", contact.Bare)
                    .Where(Exp.Eq("subs", (int)SubscriptionType.to) | Exp.Eq("subs", (int)SubscriptionType.both));
                return db.ExecList(q)
                    .Select(r => new Jid((string)r[0]))
                    .ToList();
            }
        }

        public void SaveRosterItem(Jid user, RosterItem ri)
        {
            Args.NotNull(user, "user");
            Args.NotNull(ri, "ri");
            using (var db = GetDb())
            {
                var i = new SqlInsert("jabber_roster", true)
                    .InColumnValue("user_jid", user.Bare)
                    .InColumnValue("contact_jid", ri.Jid.Bare)
                    .InColumnValue("subs", (int)ri.Subscription)
                    .InColumnValue("ask", (int)ri.Ask)
                    .InColumnValue("item", ri.ToString());
                db.ExecuteNonQuery(i);
            }
        }

        public bool RemoveRosterItem(Jid user, Jid contact)
        {
            Args.NotNull(user, "user");
            Args.NotNull(contact, "contact");
            using (var db = GetDb())
            {
                var d = new SqlDelete("jabber_roster").Where("user_jid", user.Bare).Where("contact_jid", contact.Bare);
                return 0 < db.ExecuteNonQuery(d);
            }
        }

        public IEnumerable<Jid> GetAskers(Jid contact)
        {
            Args.NotNull(contact, "contact");
            using (var db = GetDb())
            {
                return db
                    .ExecList(new SqlQuery("jabber_roster").Select("user_jid").Where("contact_jid", contact.Bare).Where(!Exp.Eq("ask", AskType.NONE)))
                    .Select(r => new Jid((string)r[0]))
                    .ToArray();
            }
        }


        private void CheckUsername(string username)
        {
            Args.Requires<ArgumentException>(!string.IsNullOrEmpty(username), "User name can not be empty.");
        }

        private DbManager GetDb()
        {
            return new DbManager(connectionStringName);
        }

        private void CreateSchema()
        {
            var jabber_user = new SqlCreate.Table("jabber_user", true)
                .AddColumn(new SqlCreate.Column("username", DbType.String, 1071).NotNull(true).PrimaryKey(true))
                .AddColumn(new SqlCreate.Column("userpass", DbType.String, 128).NotNull(true))
                .AddColumn(new SqlCreate.Column("uservcard", DbType.String, UInt16.MaxValue).NotNull(false));

            var jabber_roster = new SqlCreate.Table("jabber_roster", true)
               .AddColumn(new SqlCreate.Column("user_jid", DbType.String, 3071).NotNull(true))
               .AddColumn(new SqlCreate.Column("contact_jid", DbType.String, 3071).NotNull(true))
               .AddColumn(new SqlCreate.Column("subs", DbType.Int32).NotNull(true))
               .AddColumn(new SqlCreate.Column("ask", DbType.Int32).NotNull(true))
               .AddColumn(new SqlCreate.Column("item", DbType.String, UInt16.MaxValue).NotNull(false))
               .PrimaryKey("user_jid", "contact_jid")
               .AddIndex(new SqlCreate.Index("contact_ask", "jabber_roster", "contact_jid", "ask"));

            using (var db = GetDb())
            {
                db.ExecuteNonQuery(jabber_user);
                db.ExecuteNonQuery(jabber_roster);
            }
        }
    }
}
