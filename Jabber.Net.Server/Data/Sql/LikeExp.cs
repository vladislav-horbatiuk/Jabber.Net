#region usings

using System;

#endregion

namespace Jabber.Net.Server.Data.Sql
{
    [Flags]
    public enum SqlLike
    {
        None,
        StartWith = 1,
        EndWith = 2,
        AnyWhere = StartWith | EndWith,
    }

    public class LikeExp : Exp
    {
        private readonly string column;
        private readonly string str;

        public LikeExp(string column, string str, SqlLike like)
        {
            this.column = column;
            if (str != null && like != SqlLike.None)
            {
                if ((like & SqlLike.StartWith) == SqlLike.StartWith) str += "%";
                if ((like & SqlLike.EndWith) == SqlLike.EndWith) str = "%" + str;
            }
            this.str = str;
        }

        public override string ToString(ISqlDialect dialect)
        {
            return str != null
                       ? string.Format("{0} {1}like ?", column, Not ? "not " : string.Empty)
                       : string.Format("{0} is {1}null", column, Not ? "not " : string.Empty);
        }

        public override object[] GetParameters()
        {
            return str == null ? new object[0] : new object[] {str};
        }
    }
}