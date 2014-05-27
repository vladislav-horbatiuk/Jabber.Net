using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP.protocol.client;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Storages;

namespace Jabber.Net.Server.S2C
{
    class ClientMessageHandler : XmppHandler, IXmppHandler<Message>
    {
        private readonly Solution NO_ACCOUNT = new Solution();
        private readonly Solution NO_RESOURCES = new Solution();
        private readonly Solution ZERO_NON_NEGATIVE = new Solution();
        private readonly Solution ONE_NON_NEGATIVE = new Solution();
        private readonly Solution MORE_ONE_NON_NEGATIVE = new Solution();


        /// <summary>
        /// Exchanging Messages.
        /// </summary>
        /// <see cref="http://xmpp.org/rfcs/rfc6121.html#rules-local-message"/>
        /// <remarks>
        /// Summary of Message Delivery Rules:
        /// +----------------------------------------------------------+
        /// | Condition        | Normal | Chat  | Groupchat | Headline |
        /// +----------------------------------------------------------+
        /// | ACCOUNT DOES NOT EXIST                                   |
        /// |  bare            |   S    |   S   |     E     |    S     |
        /// |  full            |   S    |   S   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// | ACCOUNT EXISTS, BUT NO ACTIVE RESOURCES                  |
        /// |  bare            |   O    |   O   |     E     |    S     |
        /// |  full            |   S    |   O   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// | 1+ NEGATIVE RESOURCES BUT ZERO NON-NEGATIVE RESOURCES    |
        /// |  bare            |   O    |   O   |     E     |    S     |
        /// |  full match      |   D    |   D   |     D     |    D     |
        /// |  full no match   |   S    |   O   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// | 1 NON-NEGATIVE RESOURCE                                  |
        /// |  bare            |   D    |   D   |     E     |    D     |
        /// |  full match      |   D    |   D   |     D     |    D     |
        /// |  full no match   |   S    |   D   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// | 1+ NON-NEGATIVE RESOURCES                                |
        /// |  bare            |   M    |   M   |     E     |    A     |
        /// |  full match      |   D    |   D   |     D     |    D     |
        /// |  full no match   |   S    |   M   |     S     |    S     |
        /// +----------------------------------------------------------+
        /// </remarks>
        public ClientMessageHandler()
        {
            NO_ACCOUNT.SetSolution(JidType.Bare, SolutionResult.E, MessageType.groupchat);

            NO_RESOURCES.SetSolution(JidType.Bare, SolutionResult.O, MessageType.normal, MessageType.chat);
            NO_RESOURCES.SetSolution(JidType.Bare, SolutionResult.E, MessageType.groupchat);
            NO_RESOURCES.SetSolution(JidType.FullNoMatch, SolutionResult.O, MessageType.chat);

            ZERO_NON_NEGATIVE.SetSolution(JidType.Bare, SolutionResult.O, MessageType.normal, MessageType.chat);
            ZERO_NON_NEGATIVE.SetSolution(JidType.Bare, SolutionResult.E, MessageType.groupchat);
            ZERO_NON_NEGATIVE.SetSolution(JidType.FullMatch, SolutionResult.D, MessageType.normal, MessageType.chat, MessageType.groupchat, MessageType.headline);
            ZERO_NON_NEGATIVE.SetSolution(JidType.FullNoMatch, SolutionResult.O, MessageType.chat);

            ONE_NON_NEGATIVE.SetSolution(JidType.Bare, SolutionResult.D, MessageType.normal, MessageType.chat, MessageType.headline);
            ONE_NON_NEGATIVE.SetSolution(JidType.Bare, SolutionResult.E, MessageType.groupchat);
            ONE_NON_NEGATIVE.SetSolution(JidType.FullMatch, SolutionResult.D, MessageType.normal, MessageType.chat, MessageType.groupchat, MessageType.headline);
            ONE_NON_NEGATIVE.SetSolution(JidType.FullNoMatch, SolutionResult.D, MessageType.chat);

            MORE_ONE_NON_NEGATIVE.SetSolution(JidType.Bare, SolutionResult.M, MessageType.normal, MessageType.chat);
            MORE_ONE_NON_NEGATIVE.SetSolution(JidType.Bare, SolutionResult.E, MessageType.groupchat);
            MORE_ONE_NON_NEGATIVE.SetSolution(JidType.Bare, SolutionResult.A, MessageType.headline);
            MORE_ONE_NON_NEGATIVE.SetSolution(JidType.FullMatch, SolutionResult.D, MessageType.normal, MessageType.chat, MessageType.groupchat, MessageType.headline);
            MORE_ONE_NON_NEGATIVE.SetSolution(JidType.FullNoMatch, SolutionResult.D, MessageType.chat);
        }


        public XmppHandlerResult ProcessElement(Message message, XmppSession session, XmppHandlerContext context)
        {
            if (!message.HasTo)
            {
                return Error(session, ErrorCondition.BadRequest, message);
            }

            Solution solution = null;
            var jidType = message.To.IsBare ? JidType.Bare : JidType.FullNoMatch;
            var sessions = Enumerable.Empty<XmppSession>();

            if (context.Storages.Users.GetUser(message.To.User) == null)
            {
                solution = NO_ACCOUNT;
            }
            else
            {
                sessions = context.Sessions.GetSessions(message.To.BareJid);
                if (!sessions.Any())
                {
                    solution = NO_RESOURCES;
                }
                else
                {
                    sessions = sessions.Where(s => 0 <= s.Priority);
                    var nonNegativeCount = sessions.Count();
                    if (nonNegativeCount == 0)
                    {
                        solution = ZERO_NON_NEGATIVE;
                    }
                    else if (nonNegativeCount == 1)
                    {
                        solution = ONE_NON_NEGATIVE;
                    }
                    else
                    {
                        solution = MORE_ONE_NON_NEGATIVE;
                    }
                    if (message.To.IsFull && sessions.Any(s => s.Jid == message.To))
                    {
                        jidType = JidType.FullMatch;
                    }
                }
            }

            if (solution == null)
            {
                throw new InvalidOperationException("Solution not found.");
            }

            var result = solution.GetSolution(jidType, message.Type);
            switch (result)
            {
                case SolutionResult.E:
                    return Error(session, ErrorCondition.ServiceUnavailable, message);

                case SolutionResult.O:
                    context.Storages.Elements.SaveOffline(message.To, message);
                    return Void();

                case SolutionResult.D:
                    return Send(sessions.FirstOrDefault(s => s.Jid == message.To) ?? sessions.First(), message, true);

                case SolutionResult.M:
                    return Send(sessions.OrderByDescending(s => s.Priority).First(), message, true);

                case SolutionResult.A:
                    return Send(sessions, true, message);

                default:
                    return Void();
            }
        }


        private enum SolutionResult
        {
            /// <summary>
            /// Silently ignoring the message
            /// </summary>
            S,

            /// <summary>
            /// Bouncing the message with a stanza error
            /// </summary>
            E,

            /// <summary>
            /// Storing the message offline 
            /// </summary>
            O,

            /// <summary>
            /// Delivering the message to the resource specified in the 'to' address
            /// </summary>
            D,

            /// <summary>
            /// Delivering the message to the "most available" resource or resources according to the server's implementation-specific algorithm, e.g., 
            /// treating the resource or resources with the highest presence priority as "most available"
            /// </summary>
            M,

            /// <summary>
            /// Delivering the message to all resources with non-negative presence priority
            /// </summary>
            A,
        }

        private enum JidType
        {
            /// <summary>
            /// Bare JID
            /// </summary>
            Bare,

            /// <summary>
            /// Full JID matching no available resource
            /// </summary>
            FullNoMatch,

            /// <summary>
            /// Full JID matching an available resource
            /// </summary>
            FullMatch,
        }

        private class Solution
        {
            private readonly IDictionary<string, SolutionResult> results = new Dictionary<string, SolutionResult>(5);

            public SolutionResult GetSolution(JidType jidType, MessageType messageType)
            {
                SolutionResult result;
                return results.TryGetValue(jidType.ToString() + messageType.ToString(), out result) ? result : default(SolutionResult);
            }

            public void SetSolution(JidType jidType, SolutionResult result, params MessageType[] messageTypes)
            {
                foreach (var messageType in messageTypes)
                {
                    results[jidType.ToString() + messageType.ToString()] = result;
                }
            }
        }
    }
}
