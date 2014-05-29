namespace Jabber.Net.Server.Data.Sql
{
    class SqlIdentifier : ISqlInstruction
    {
        public string Identifier
        {
            get;
            private set;
        }


        public SqlIdentifier(string identifier)
        {
            Identifier = identifier;
        }

        public string ToString(ISqlDialect dialect)
        {
            return Identifier;
        }

        public object[] GetParameters()
        {
            return new object[0];
        }

        public static explicit operator SqlIdentifier(string identifier)
        {
            return new SqlIdentifier(identifier);
        }
    }
}