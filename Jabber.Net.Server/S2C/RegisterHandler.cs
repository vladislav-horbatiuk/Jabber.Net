using agsXMPP.Idn;
using agsXMPP.protocol;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.register;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Resources;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class RegisterHandler : XmppHandler, IXmppHandler<RegisterIq>, IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.Handlers.SupportRegister = true;
        }

        [IQ(IqType.get, IqType.set)]
        public XmppHandlerResult ProcessElement(RegisterIq element, XmppSession session, XmppHandlerContext context)
        {
            if (element.Type == IqType.get)
            {
                element.Query.Instructions = RS.RegisterInstructions;
                element.Query.Username = string.Empty;
                element.Query.Password = string.Empty;
                element.ToResult();
                if (session.Jid.HasUser && context.Storages.Users.GetUser(session.Jid.User) != null)
                {
                    element.Query.Username = session.Jid.User;
                    element.Query.AddChild(new Element("registered"));
                }
                else
                {
                    element.To = null;
                }
                element.From = null;
                return Send(session, element);
            }
            else
            {
                if (element.Query.RemoveAccount)
                {
                    if (!session.Authenticated || !session.Jid.HasUser)
                    {
                        return Error(session, StreamErrorCondition.NotAuthorized);
                    }

                    context.Storages.Users.RemoveUser(session.Jid.User);
                    var errors = Component();
                    foreach (var s in context.Sessions.GetSessions(session.Jid.BareJid))
                    {
                        if (!session.Equals(s))
                        {
                            errors.Add(Error(s, StreamErrorCondition.NotAuthorized));
                        }
                    }

                    element.Query.Remove();
                    element.ToResult();
                    element.From = element.To = null;
                    return Component(Send(session, element), errors);
                }

                if (string.IsNullOrEmpty(element.Query.Username))
                {
                    return Error(session, ErrorCondition.NotAcceptable, element, RS.RegisterEmptyUsername);
                }
                if (string.IsNullOrEmpty(element.Query.Password))
                {
                    return Error(session, ErrorCondition.NotAcceptable, element, RS.RegisterEmptyPassword);
                }
                if (Stringprep.NamePrep(element.Query.Username) != element.Query.Username)
                {
                    return Error(session, ErrorCondition.NotAcceptable, element, RS.RegisterInvalidCharacter);
                }

                var user = context.Storages.Users.GetUser(element.Query.Username);
                if (user != null && user.Name != session.Jid.User)
                {
                    return Error(session, ErrorCondition.Conflict, element);
                }

                context.Storages.Users.SaveUser(new XmppUser(element.Query.Username, element.Query.Password));

                element.Query.Remove();
                element.ToResult();
                if (!session.Authenticated)
                {
                    element.To = null;
                }
                element.From = null;
                return Send(session, element);
            }
        }
    }
}
