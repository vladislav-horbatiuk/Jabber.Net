using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using agsXMPP;
using agsXMPP.util;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Data;
using Jabber.Net.Server.Data.Sql;

namespace Jabber.Net.Server.Storages
{
    public class XmppElementStorage : IXmppElementStorage
    {
        private readonly string connectionStringName;


        public XmppElementStorage(string connectionStringName)
        {
            Args.NotNull(connectionStringName, "connectionStringName");
            this.connectionStringName = connectionStringName;

            CreateSchema();
        }


        public IEnumerable<Element> GetElements(Jid jid, string key)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(key, "key");

            var q = new SqlQuery("jabber_element")
                .Select("element_text")
                .Where("jid", jid.Bare)
                .Where(key.Contains('%') ? Exp.Like("element_key", key) : Exp.Eq("element_key", key));
            using (var db = GetDb())
            {
                return db.ExecList(q)
                    .Select(r => ElementSerializer.DeSerializeElement<Element>((string)r[0]))
                    .ToList();
            }
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

            using (var db = GetDb())
            {
                var i = new SqlInsert("jabber_element", true)
                    .InColumnValue("jid", jid.Bare)
                    .InColumnValue("element_key", key)
                    .InColumnValue("element_text", element.ToString());
                db.ExecuteNonQuery(i);
            }
        }


        public bool RemoveElements(Jid jid, string key)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(key, "key");

            using (var db = GetDb())
            {
                var d = new SqlDelete("jabber_element")
                    .Where("jid", jid.Bare)
                    .Where(key.Contains('%') ? Exp.Like("element_key", key) : Exp.Eq("element_key", key));
                return 0 < db.ExecuteNonQuery(d);
            }
        }


        private void CreateSchema()
        {
            var jabber_element = new SqlCreate.Table("jabber_element", true)
                .AddColumn(new SqlCreate.Column("jid", DbType.String, 3071).NotNull(true))
                .AddColumn(new SqlCreate.Column("element_key", DbType.String, 1024).NotNull(true))
                .AddColumn(new SqlCreate.Column("element_text", DbType.String, UInt16.MaxValue).NotNull(false))
                .PrimaryKey("jid", "element_key");

            using (var db = GetDb())
            {
                db.ExecuteNonQuery(jabber_element);
            }
        }

        private DbManager GetDb()
        {
            return new DbManager(connectionStringName);
        }
    }
}
