using System.Collections.Generic;
using System.Linq;
using agsXMPP;
using Jabber.Net.Server.Collections;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSessionManager
    {
        private readonly ReaderWriterLockDictionary<string, XmppSession> sessions = new ReaderWriterLockDictionary<string, XmppSession>(1000);


        public XmppSession GetSession(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            XmppSession s;
            return sessions.TryGetValue(id, out s) ? s : null;
        }

        public XmppSession GetSession(Jid jid)
        {
            Args.NotNull(jid, "jid");
            return sessions.Values.SingleOrDefault(s => s.Jid == jid);
        }

        public IEnumerable<XmppSession> GetSessions(Jid jid)
        {
            Args.NotNull(jid, "jid");
            return jid.IsBare ?
                sessions.Values.Where(s => s.Binded && s.Jid.BareJid == jid.BareJid).ToList() :
                sessions.Values.Where(s => s.Jid == jid).ToList();
        }

        public void OpenSession(XmppSession session)
        {
            Args.NotNull(session, "session");
            Log.Information("Open session {0}", session.Id);

            sessions[session.Id] = session;
        }

        public void CloseSession(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                XmppSession s;
                sessions.Remove(id, out s);
                if (s != null)
                {
                    Log.Information("Close session {0}", id);
                    s.Connection.Close();
                }
            }
        }
    }
}
