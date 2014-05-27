using System.Collections;
using System.Data;
using System.Text;

namespace Jabber.Net.Server.Data
{
    public class SqlDialect : ISqlDialect
    {
        public static readonly ISqlDialect Default = new SqlDialect();

        private static readonly Hashtable dialects = Hashtable.Synchronized(new Hashtable());


        public virtual string IdentityQuery
        {
            get { return "@@identity"; }
        }

        public virtual string Autoincrement
        {
            get { return "AUTOINCREMENT"; }
        }

        public virtual string InsertIgnore
        {
            get { return "insert ignore"; }
        }


        public virtual string DbTypeToString(DbType type, int size, int precision)
        {
            var s = new StringBuilder(type.ToString().ToLower());
            if (0 < size)
            {
                s.AppendFormat(0 < precision ? "({0}, {1})" : "({0})", size, precision);
            }
            return s.ToString();
        }


        static SqlDialect()
        {
            RegisterDialect("System.Data.SQLite", new SQLiteDialect());
            RegisterDialect("MySql.Data.MySqlClient", new MySQLDialect());
        }


        public static void RegisterDialect(string providerName, ISqlDialect dialect)
        {
            dialects[providerName] = dialect;
        }

        public static ISqlDialect GetDialect(string providerName)
        {
            return dialects[providerName] as ISqlDialect;
        }
    }
}