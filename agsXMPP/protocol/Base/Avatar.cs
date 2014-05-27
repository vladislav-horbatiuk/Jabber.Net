// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Avatar.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.Base
{
    #region usings

    using System;
    using Xml.Dom;

    #endregion

    // Avatar is in multiple Namespaces. So better to work with a Base class

    /// <summary>
    /// Summary description for Avatar.
    /// </summary>
    public class Avatar : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Avatar()
        {
            TagName = "query";
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public byte[] Data
        {
            get
            {
                if (HasTag("data"))
                {
                    return Convert.FromBase64String(GetTag("data"));
                }
                else
                {
                    return null;
                }
            }

            set { SetTag("data", Convert.ToBase64String(value, 0, value.Length)); }
        }

        /// <summary>
        /// </summary>
        public string MimeType
        {
            get
            {
                Element data = SelectSingleElement("data");
                if (data != null)
                {
                    return GetAttribute("mimetype");
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Element data = SelectSingleElement("data");
                if (data != null)
                {
                    SetAttribute("mimetype", value);
                }
            }
        }

        #endregion
    }
}