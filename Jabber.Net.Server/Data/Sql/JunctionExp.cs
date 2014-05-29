#region usings

using System.Collections.Generic;

#endregion

namespace Jabber.Net.Server.Data.Sql
{
    public class JunctionExp : Exp
    {
        private readonly bool and;
        private readonly Exp exp1;
        private readonly Exp exp2;

        public JunctionExp(Exp exp1, Exp exp2, bool and)
        {
            this.exp1 = exp1;
            this.exp2 = exp2;
            this.and = and;
        }

        public override string ToString(ISqlDialect dialect)
        {
            string format = exp1 is JunctionExp && ((JunctionExp) exp1).and != and ? "({0})" : "{0}";
            format += " {1} ";
            format += exp2 is JunctionExp && ((JunctionExp) exp2).and != and ? "({2})" : "{2}";
            return Not
                       ? string.Format(format, (!exp1).ToString(dialect), and ? "or" : "and",
                                       (!exp2).ToString(dialect))
                       : string.Format(format, exp1.ToString(dialect), and ? "and" : "or", exp2.ToString(dialect));
        }

        public override object[] GetParameters()
        {
            var parameters = new List<object>();
            parameters.AddRange(exp1.GetParameters());
            parameters.AddRange(exp2.GetParameters());
            return parameters.ToArray();
        }
    }
}