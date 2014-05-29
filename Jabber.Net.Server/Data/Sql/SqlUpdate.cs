#region usings

using System.Collections.Generic;
using System.Text;
using Jabber.Net.Server.Data.Sql;

#endregion

namespace Jabber.Net.Server.Data.Sql
{
    public class SqlUpdate : ISqlInstruction
    {
        private readonly List<string> expressions = new List<string>();
        private readonly Dictionary<string, object> sets = new Dictionary<string, object>();
        private readonly SqlIdentifier table;
        private Exp where = Exp.Empty;

        public SqlUpdate(string table)
        {
            this.table = (SqlIdentifier) table;
        }

        #region ISqlInstruction Members

        public string ToString(ISqlDialect dialect)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("update {0} set ", table.ToString(dialect));
            foreach (string column in sets.Keys)
            {
                sql.AppendFormat("{0} = ?, ", column);
            }
            expressions.ForEach(expression => sql.AppendFormat("{0}, ", expression));
            sql.Remove(sql.Length - 2, 2);
            if (where != Exp.Empty) sql.AppendFormat(" where {0}", where.ToString(dialect));
            return sql.ToString();
        }

        public object[] GetParameters()
        {
            var parameters = new List<object>();
            foreach (object parameter in sets.Values)
            {
                parameters.Add(parameter);
            }
            if (where != Exp.Empty) parameters.AddRange(where.GetParameters());
            return parameters.ToArray();
        }

        #endregion

        public SqlUpdate Set(string expression)
        {
            expressions.Add(expression);
            return this;
        }

        public SqlUpdate Set(string column, object value)
        {
            sets[column] = value;
            return this;
        }

        public SqlUpdate Where(Exp where)
        {
            this.where = this.where & where;
            return this;
        }

        public SqlUpdate Where(string column, object value)
        {
            return Where(Exp.Eq(column, value));
        }

        public override string ToString()
        {
            return ToString(SqlDialect.Default);
        }
    }
}