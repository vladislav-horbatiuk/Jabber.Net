// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="StartTls.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.tls
{
    #region usings

    using Xml.Dom;

    #endregion

    // Step 4: Client sends the STARTTLS command to server:
    // <starttls xmlns='urn:ietf:params:xml:ns:xmpp-tls'/>

    /// <summary>
    /// Summary description for starttls.
    /// </summary>
    public class StartTls : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public StartTls()
        {
            TagName = "starttls";
            Namespace = Uri.TLS;
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public bool Required
        {
            get { return HasTag("required"); }

            set
            {
                if (value == false)
                {
                    if (HasTag("required"))
                    {
                        RemoveTag("required");
                    }
                }
                else
                {
                    if (!HasTag("required"))
                    {
                        SetTag("required");
                    }
                }
            }
        }

        #endregion
    }
}