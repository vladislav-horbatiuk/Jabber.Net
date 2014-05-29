#region usings

using System.Text;
using Jabber.Net.Server.Data.Sql;

#endregion

namespace Jabber.Net.Server.Data.Sql
{
    public class SqlDelete : ISqlInstruction
    {
        private readonly string table;
        private Exp where = Exp.Empty;

        public SqlDelete(string table)
        {
            this.table = table;
        }

        #region ISqlInstruction Members

        public string ToString(ISqlDialect dialect)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("delete from {0}", table);
            if (where != Exp.Empty) sql.AppendFormat(" where {0}", where.ToString(dialect));
            return sql.ToString();
        }

        public object[] GetParameters()
        {
            return where != Exp.Empty ? where.GetParameters() : new object[0];
        }

        #endregion

        public SqlDelete Where(Exp where)
        {
            this.where = this.where & where;
            return this;
        }

        public SqlDelete Where(string column, object value)
        {
            return Where(Exp.Eq(column, value));
        }

        public override string ToString()
        {
            return ToString(SqlDialect.Default);
        }
    }
}