namespace Jabber.Net.Server.Data.Sql
{
    public class SqlExp : Exp
    {
        private readonly string sql;

        public SqlExp(string sql)
        {
            this.sql = sql;
        }

        public override string ToString(ISqlDialect dialect)
        {
            return sql;
        }
    }
}