namespace Jabber.Net.Server.Data.Sql
{
    public class EqExp : Exp
    {
        private readonly string column;
        private readonly object value;

        public EqExp(string column, object value)
        {
            this.column = column;
            this.value = value;
        }

        public override string ToString(ISqlDialect dialect)
        {
            return string.Format("{0} {1}",
                                 column,
                                 value != null ? (Not ? "<> ?" : "= ?") : (Not ? "is not null" : "is null"));
        }

        public override object[] GetParameters()
        {
            return value == null ? new object[0] : new[] {value};
        }
    }
}