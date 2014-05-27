using System;
using System.Data;

namespace Jabber.Net.Server.Data
{
    class SQLiteDialect : ISqlDialect
    {
        public string IdentityQuery
        {
            get { return "last_insert_rowid()"; }
        }

        public string Autoincrement
        {
            get { return "autoincrement"; }
        }

        public virtual string InsertIgnore
        {
            get { return "insert or ignore"; }
        }

        public string DbTypeToString(DbType type, int size, int precision)
        {
            switch (type)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                case DbType.Guid:
                    return "TEXT";

                case DbType.Binary:
                case DbType.Object:
                    return "BLOB";

                case DbType.Boolean:
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.VarNumeric:
                    return "NUMERIC";

                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.Time:
                    return "DATETIME";

                case DbType.Byte:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                    return "INTEGER";

                case DbType.Double:
                case DbType.Single:
                    return "REAL";
            }
            throw new ArgumentOutOfRangeException(type.ToString());
        }
    }
}