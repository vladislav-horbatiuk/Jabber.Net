namespace agsXMPP.protocol.iq.jingle
{
    using Xml.Dom;

    public class Stun : Element
    {
        public Stun()
        {
            TagName = "stun";
            Namespace = Uri.IQ_GOOGLE_JINGLE;
        }
        public Server[] GetServers()
        {
            ElementList nl = SelectElements(typeof(Server));
            int i = 0;
            Server[] result = new Server[nl.Count];
            foreach (Server ri in nl)
            {
                result[i] = (Server)ri;
                i++;
            }
            return result;
        }

        public void AddServer(Server r)
        {
            this.ChildNodes.Add(r);
        }
    }

    public class Server : Element
    {
        public Server()
        {
            TagName = "server";
            Namespace = Uri.IQ_GOOGLE_JINGLE;
        }

        public Server(string host, int udp):this()
        {
            Host = host;
            Udp = udp;
        }

        public string Host
        {
            get { return GetAttribute("host"); }

            set { SetAttribute("host", value); }
        }

        public int Udp
        {
            get { return GetAttributeInt("udp"); }

            set { SetAttribute("udp", value); }
        }
    }


    public class GoogleJingle : Element
    {
        public GoogleJingle()
        {
            TagName = "query";
            Namespace	= Uri.IQ_GOOGLE_JINGLE;
        }


        public virtual Stun Stun
        {
            get { return SelectSingleElement(typeof(Stun)) as Stun; }

            set
            {
                RemoveTag(typeof(Stun));
                if (value != null)
                {
                    AddChild(value);
                }
            }
        }
    }


}