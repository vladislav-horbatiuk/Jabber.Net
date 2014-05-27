
namespace Jabber.Net.Server
{
    public class XmppUser
    {
        public string Name
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }


        public XmppUser(string name, string password)
        {
            Args.NotNull(name, "name");
            Args.NotNull(password, "password");

            Name = name;
            Password = password;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var u = obj as XmppUser;
            return u != null && Equals(Name, u.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
