// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Failure.cs">
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

    // Step 5 (alt): Server informs client that TLS negotiation has failed and closes both stream and TCP connection:

    // <failure xmlns='urn:ietf:params:xml:ns:xmpp-tls'/>
    // </stream:stream>

    /// <summary>
    /// Summary description for Failure.
    /// </summary>
    public class Failure : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Failure()
        {
            TagName = "failure";
            Namespace = Uri.TLS;
        }

        #endregion
    }
}