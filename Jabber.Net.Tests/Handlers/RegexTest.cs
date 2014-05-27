using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Jabber.Net.Tests.Handlers
{
    [TestFixture]
    public class RegexTest
    {
        public void JidRegex()
        {
            var regex = new Regex(".*", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var r = regex.IsMatch("s");
            r = regex.IsMatch("u@s");
            r = regex.IsMatch("u@s/R");

            regex = new Regex("^s$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            r = regex.IsMatch("s");
            r = regex.IsMatch("u@s");
            r = regex.IsMatch("u@s/R");

            regex = new Regex(".+@s.*", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            r = regex.IsMatch("s");
            r = regex.IsMatch("u@s");
            r = regex.IsMatch("u@s/R");

            regex = new Regex(".+@s/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            r = regex.IsMatch("s");
            r = regex.IsMatch("u@s");
            r = regex.IsMatch("u@s/R");

            regex = new Regex("", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            r = regex.IsMatch("s");
            r = regex.IsMatch("u@s");
            r = regex.IsMatch("u@s/R");
        }
    }
}
