// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Role.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc
{
    using System;

    /// <summary>
    /// There are four defined roles that an occupant may have
    /// </summary>
    [Flags]
    public enum Role
    {
        /// <summary>
        /// the absence of a role
        /// </summary>
        none, 

        /// <summary>
        /// </summary>
        moderator, 

        /// <summary>
        /// </summary>
        participant, 

        /// <summary>
        /// </summary>
        visitor
    }
}