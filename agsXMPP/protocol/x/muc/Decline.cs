// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Decline.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc
{
    /*
    Example 45. Invitee Declines Invitation

    <message
        from='hecate@shakespeare.lit/broom'
        to='darkcave@macbeth.shakespeare.lit'>
      <x xmlns='http://jabber.org/protocol/muc#user'>
        <decline to='crone1@shakespeare.lit'>
          <reason>
            Sorry, I'm too busy right now.
          </reason>
        </decline>
      </x>
    </message>
        

    Example 46. Room Informs Invitor that Invitation Was Declined

    <message
        from='darkcave@macbeth.shakespeare.lit'
        to='crone1@shakespeare.lit/desktop'>
      <x xmlns='http://jabber.org/protocol/muc#user'>
        <decline from='hecate@shakespeare.lit'>
          <reason>
            Sorry, I'm too busy right now.
          </reason>
        </decline>
      </x>
    </message>
    */

    /// <summary>
    /// </summary>
    public class Decline : Invitation
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Decline() : base()
        {
            TagName = "decline";
        }

        /// <summary>
        /// </summary>
        /// <param name="reason">
        /// </param>
        public Decline(string reason) : this()
        {
            Reason = reason;
        }

        /// <summary>
        /// </summary>
        /// <param name="to">
        /// </param>
        public Decline(Jid to) : this()
        {
            To = to;
        }

        /// <summary>
        /// </summary>
        /// <param name="to">
        /// </param>
        /// <param name="reason">
        /// </param>
        public Decline(Jid to, string reason) : this()
        {
            To = to;
            Reason = reason;
        }

        #endregion
    }
}