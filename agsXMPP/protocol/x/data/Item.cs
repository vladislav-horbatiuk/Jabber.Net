// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Item.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.data
{
    /// <summary>
    /// Used in XData seach.
    /// includes the headers of the search results
    /// </summary>
    public class Item : FieldContainer
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Item()
        {
            TagName = "item";
            Namespace = Uri.X_DATA;
        }

        #endregion
    }
}