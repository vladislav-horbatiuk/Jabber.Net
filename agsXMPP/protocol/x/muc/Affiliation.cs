// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Affiliation.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc
{
    /// <summary>
    /// There are five defined affiliations that a user may have in relation to a room
    /// </summary>
    public enum Affiliation
    {
        /// <summary>
        /// the absence of an affiliation
        /// </summary>
        none, 

        /// <summary>
        /// </summary>
        owner, 

        /// <summary>
        /// </summary>
        admin, 

        /// <summary>
        /// </summary>
        member, 

        /// <summary>
        /// </summary>
        outcast
    }
}