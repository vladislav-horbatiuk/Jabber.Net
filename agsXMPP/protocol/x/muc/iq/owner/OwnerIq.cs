// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="OwnerIq.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc.iq.owner
{
    #region usings

    using client;

    #endregion

    /*
        Example 72. Moderator Kicks Occupant

        <iq from='fluellen@shakespeare.lit/pda'
            id='kick1'
            to='harfleur@henryv.shakespeare.lit'
            type='set'>
          <query xmlns='http://jabber.org/protocol/muc#admin'>
            <item nick='pistol' role='none'>
              <reason>Avaunt, you cullion!</reason>
            </item>
          </query>
        </iq>
    */

    /// <summary>
    /// </summary>
    public class OwnerIq : IQ
    {
        #region Members

        /// <summary>
        /// </summary>
        private Owner m_Owner = new Owner();

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        public OwnerIq()
        {
            base.Query = m_Owner;
            GenerateId();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public OwnerIq(IqType type) : this()
        {
            Type = type;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="to">
        /// </param>
        public OwnerIq(IqType type, Jid to) : this(type)
        {
            To = to;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="to">
        /// </param>
        /// <param name="from">
        /// </param>
        public OwnerIq(IqType type, Jid to, Jid from) : this(type, to)
        {
            From = from;
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public new Owner Query
        {
            get { return m_Owner; }
        }

        #endregion
    }
}