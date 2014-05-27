using System;
using agsXMPP.protocol.sasl;
using agsXMPP.sasl.DigestMD5;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;

namespace Jabber.Net.Server.S2C
{
    class AuthDigestMD5Handler : XmppHandler,
        IXmppHandler<Auth>,
        IXmppHandler<Response>,
        IXmppHandler<Abort>,
        IXmppRegisterHandler
    {
        public void OnRegister(XmppHandlerContext context)
        {
            context.Handlers.SupportedAuthMechanisms.Add(new Mechanism(MechanismType.DIGEST_MD5, true));
        }


        public XmppHandlerResult ProcessElement(Auth element, XmppSession session, XmppHandlerContext context)
        {
            if (element.MechanismType != MechanismType.DIGEST_MD5)
            {
                return Error(session, FailureCondition.invalid_mechanism);
            }

            var authStep = session.AuthData as AuthData;
            if (authStep != null)
            {
                return Error(session, FailureCondition.temporary_auth_failure);
            }

            session.AuthData = new AuthData();
            var challenge = new Challenge
            {
                TextBase64 = string.Format("realm=\"{0}\",nonce=\"{1}\",qop=\"auth\",charset=utf-8,algorithm=md5-sess", session.Jid.Server, CreateId())
            };
            return Send(session, challenge);
        }

        public XmppHandlerResult ProcessElement(Response element, XmppSession session, XmppHandlerContext context)
        {
            var authStep = session.AuthData as AuthData;
            if (authStep == null)
            {
                return Error(session, FailureCondition.temporary_auth_failure);
            }

            if (authStep.Step == AuthStep.Step1)
            {
                var step = new Step2(element.TextBase64);
                var user = context.Storages.Users.GetUser(step.Username);

                if (user != null &&
                    string.Compare(session.Jid.Server, step.Realm, StringComparison.OrdinalIgnoreCase) == 0 &&
                    step.Authorize(step.Username, user.Password))
                {
                    var challenge = new Challenge
                    {
                        TextBase64 = string.Format("rspauth={0}", step.CalculateResponse(step.Username, user.Password, string.Empty))
                    };
                    authStep.DoStep(step.Username);
                    return Send(session, challenge);
                }
                else
                {
                    return Error(session, FailureCondition.not_authorized);
                }
            }
            else if (authStep.Step == AuthStep.Step2)
            {
                session.Authenticate(authStep.UserName);
                session.Connection.Reset();
                return Send(session, new Success());
            }
            else
            {
                return Error(session, FailureCondition.temporary_auth_failure);
            }
        }

        public XmppHandlerResult ProcessElement(Abort element, XmppSession session, XmppHandlerContext context)
        {
            return Error(session, FailureCondition.aborted);
        }


        private enum AuthStep
        {
            Step1,
            Step2,
        }

        private class AuthData
        {
            public string UserName
            {
                get;
                private set;
            }

            public AuthStep Step
            {
                get;
                private set;
            }

            public void DoStep(string username)
            {
                UserName = username;
                Step++;
            }
        }
    }
}
