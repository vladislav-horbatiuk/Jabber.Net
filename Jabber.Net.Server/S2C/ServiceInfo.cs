using agsXMPP;

namespace Jabber.Net.Server.S2C
{
    public class ServiceInfo
    {
        public Jid Jid
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Category
        {
            get;
            private set;
        }

        public string Type
        {
            get;
            private set;
        }

        public string Url
        {
            get;
            set;
        }

        public string Copyrigth
        {
            get;
            set;
        }

        public string[] Features
        {
            get;
            set;
        }


        public ServiceInfo(Jid jid, string name, string category, string type)
        {
            Args.NotNull(jid, "jid");
            Args.NotNull(name, "name");
            Args.NotNull(category, "category");
            Args.NotNull(type, "type");

            Jid = jid;
            Name = name;
            Category = category;
            Type = type;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
