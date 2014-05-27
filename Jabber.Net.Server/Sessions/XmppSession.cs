using agsXMPP;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Utils;
using agsXMPP.protocol.client;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSession
    {
        public static readonly XmppSession Empty = new XmppSession(string.Empty);

        private static readonly IUniqueId id = new RandomUniqueId();
        private IXmppConnection connection;


        public string Id
        {
            get;
            private set;
        }

        public Jid Jid
        {
            get;
            set;
        }

        public IXmppConnection Connection
        {
            get { return connection; }
            private set
            {
                Args.NotNull(value, "Connection");
                connection = value;
                connection.SessionId = Id;
            }
        }

        public string Language
        {
            get;
            set;
        }

        public bool Authenticated
        {
            get;
            private set;
        }

        public object AuthData
        {
            get;
            set;
        }

        public bool Binded
        {
            get;
            private set;
        }

        public bool Rostered
        {
            get;
            private set;
        }

        public Presence Presence
        {
            get;
            set;
        }

        public bool Available
        {
            get { return Presence != null && Presence.Type == PresenceType.available; }
        }

        public int Priority
        {
            get { return Presence != null ? Presence.Priority : 0; }
        }


        private XmppSession(string id)
        {
            Id = id;
            Jid = Jid.Empty;
        }

        public XmppSession(IXmppConnection connection)
            : this(connection.SessionId ?? id.CreateId())
        {
            Connection = connection;
        }

        public void Authenticate(string username)
        {
            Jid.User = username;
            Authenticated = true;
            AuthData = null;
        }

        public void Bind(string resource)
        {
            Jid.Resource = resource;
            Binded = true;
        }

        public void RosterRequest()
        {
            Rostered = true;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var s = obj as XmppSession;
            return s != null && Equals(Id, s.Id);
        }
    }
}
