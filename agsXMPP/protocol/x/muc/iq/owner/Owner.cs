// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Owner.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc.iq.owner
{
    #region usings

    using Xml.Dom;

    #endregion

    /*
        <iq id="jcl_110" to="xxxxxx@conference.jabber.org" type="set">
            <query xmlns="http://jabber.org/protocol/muc#owner">
                <x type="submit" xmlns="jabber:x:data"/>
            </query>
        </iq>
    */

    /// <summary>
    /// </summary>
    public class Owner : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Owner()
        {
            TagName = "query";
            Namespace = Uri.MUC_OWNER;
        }

        #endregion
    }
}