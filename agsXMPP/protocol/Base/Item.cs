// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Item.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.Base
{
    #region usings

    using Xml.Dom;

    #endregion

    /// <summary>
    /// Summary description for Item.
    /// </summary>
    public class Item : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Item()
        {
            TagName = "item";
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public Jid Jid
        {
            get
            {
                if (HasAttribute("jid"))
                {
                    return new Jid(GetAttribute("jid"));
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (value != null)
                {
                    SetAttribute("jid", value.ToString());
                }
            }
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get { return GetAttribute("name"); }

            set { SetAttribute("name", value); }
        }

        #endregion
    }
}