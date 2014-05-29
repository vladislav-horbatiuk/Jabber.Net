namespace Jabber.Net.Server.Data.Sql
{
    public class SubExp : Exp
    {
        private readonly string alias;
        private readonly SqlQuery subQuery;

        public SubExp(SqlQuery subQuery)
            : this(subQuery, null)
        {
        }

        public SubExp(SqlQuery subQuery, string alias)
        {
            this.subQuery = subQuery;
            this.alias = alias;
        }

        public override string ToString(ISqlDialect dialect)
        {
            return string.Format("({0}){1}", subQuery.ToString(dialect),
                                 string.IsNullOrEmpty(alias) ? string.Empty : " as " + alias);
        }

        public override object[] GetParameters()
        {
            return subQuery.GetParameters();
        }
    }
}