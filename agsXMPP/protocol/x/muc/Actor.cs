// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Actor.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc
{
    #region usings

    using Xml.Dom;

    #endregion

    /// <summary>
    /// </summary>
    public class Actor : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Actor()
        {
            TagName = "actor";
            Namespace = Uri.MUC_USER;
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public Jid Jid
        {
            get { return GetAttributeJid("jid"); }

            set { SetAttribute("jid", value); }
        }

        #endregion
    }
}