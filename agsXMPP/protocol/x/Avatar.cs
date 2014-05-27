// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Avatar.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x
{
    #region usings

    using Xml.Dom;

    #endregion

    // <x xmlns="jabber:x:avatar"><hash>bbf231f2b7fa1772c2ec5cffa620d3aedb4bd793</hash></x>

    /// <summary>
    /// JEP-0008 avatars
    /// </summary>
    public class Avatar : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Avatar()
        {
            TagName = "x";
            Namespace = Uri.X_AVATAR;
        }

        /// <summary>
        /// </summary>
        /// <param name="hash">
        /// </param>
        public Avatar(string hash) : this()
        {
            Hash = hash;
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public string Hash
        {
            get { return GetTag("hash"); }

            set { SetTag("hash", value); }
        }

        #endregion
    }
}