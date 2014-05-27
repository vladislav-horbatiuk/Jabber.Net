// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Reported.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.data
{
    /// <summary>
    /// Used in XData seach reports.
    /// includes the headers of the search results
    /// </summary>
    public class Reported : FieldContainer
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Reported()
        {
            TagName = "reported";
            Namespace = Uri.X_DATA;
        }

        #endregion
    }
}