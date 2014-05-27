namespace Jabber.Net.Server.Data
{
    public interface ISqlInstruction
    {
        string ToString(ISqlDialect dialect);

        object[] GetParameters();
    }
}