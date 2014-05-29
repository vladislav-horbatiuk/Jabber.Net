namespace Jabber.Net.Server.Data.Sql
{
    public class EqColumnsExp : Exp
    {
        private readonly ISqlInstruction column1;
        private readonly ISqlInstruction column2;

        public EqColumnsExp(string column1, string column2)
        {
            this.column1 = (SqlIdentifier) column1;
            this.column2 = (SqlIdentifier) column2;
        }

        public EqColumnsExp(string column1, SqlQuery query)
        {
            this.column1 = (SqlIdentifier) column1;
            column2 = new SubExp(query);
        }

        public override string ToString(ISqlDialect dialect)
        {
            return string.Format("{0} {1} {2}",
                                 column1.ToString(dialect),
                                 Not ? "<>" : "=",
                                 column2.ToString(dialect));
        }

        public override object[] GetParameters()
        {
            return column2.GetParameters();
        }
    }
}