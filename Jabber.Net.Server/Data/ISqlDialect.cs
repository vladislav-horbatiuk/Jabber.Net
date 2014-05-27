using System.Data;

namespace Jabber.Net.Server.Data
{
    public interface ISqlDialect
    {
        string IdentityQuery { get; }

        string Autoincrement { get; }

        string InsertIgnore { get; }

        string DbTypeToString(DbType type, int size, int precision);
    }
}