// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Invite.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc
{
    #region usings

    using extensions.nickname;

    #endregion

    /*
    <message
        from='crone1@shakespeare.lit/desktop'
        to='darkcave@macbeth.shakespeare.lit'>
      <x xmlns='http://jabber.org/protocol/muc#user'>
        <invite to='hecate@shakespeare.lit'>
          <reason>
            Hey Hecate, this is the place for all good witches!
          </reason>
        </invite>
      </x>
    </message>
    */

    /// <summary>
    /// Invite other users t a chatroom
    /// </summary>
    public class Invite : Invitation
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Invite() : base()
        {
            TagName = "invite";
        }

        /// <summary>
        /// </summary>
        /// <param name="reason">
        /// </param>
        public Invite(string reason) : this()
        {
            Reason = reason;
        }

        /// <summary>
        /// </summary>
        /// <param name="to">
        /// </param>
        public Invite(Jid to) : this()
        {
            To = to;
        }

        /// <summary>
        /// </summary>
        /// <param name="to">
        /// </param>
        /// <param name="reason">
        /// </param>
        public Invite(Jid to, string reason) : this()
        {
            To = to;
            Reason = reason;
        }

        #endregion

        /*
            <invite to='wiccarocks@shakespeare.lit/laptop'>
                <reason>This coven needs both wiccarocks and hag66.</reason>
                <continue/>
            </invite>
         */
        #region Properties

        /// <summary>
        /// </summary>
        public bool Continue
        {
            get { return GetTag("continue") == null ? false : true; }

            set
            {
                if (value)
                {
                    SetTag("continue");
                }
                else
                {
                    RemoveTag("continue");
                }
            }
        }

        /// <summary>
        /// Nickname Element
        /// </summary>
        public Nickname Nickname
        {
            get { return SelectSingleElement(typeof (Nickname)) as Nickname; }

            set
            {
                if (HasTag(typeof (Nickname)))
                {
                    RemoveTag(typeof (Nickname));
                }

                if (value != null)
                {
                    AddChild(value);
                }
            }
        }

        #endregion
    }
}