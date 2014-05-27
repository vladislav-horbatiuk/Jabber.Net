// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Delay.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x
{
    #region usings

    using System;
    using util;
    using Xml.Dom;
    using Uri = Uri;

    #endregion

    // <presence to="gnauck@myjabber.net/myJabber v3.5" from="yahoo.myjabber.net/registered">
    // 		<status>Extended Away</status>
    // 		<show>xa</show><priority>5</priority>
    // 		<x stamp="20050206T13:09:50" from="gnauck@myjabber.net/myJabber v3.5" xmlns="jabber:x:delay"/>    
    // </presence> 

    /// <summary>
    /// <para>
    /// Delay class for Timestamps
    /// </para>
    /// <para>
    /// Mainly used in offline and groupchat messages. This is the time when the message was received by the server
    /// </para>
    /// </summary>
    public class Delay : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Delay()
        {
            TagName = "x";
            Namespace = Uri.X_DELAY;
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public Jid From
        {
            get
            {
                if (HasAttribute("from"))
                {
                    return new Jid(GetAttribute("from"));
                }
                else
                {
                    return null;
                }
            }

            set { SetAttribute("from", value.ToString()); }
        }

        /// <summary>
        /// </summary>
        public DateTime Stamp
        {
            get { return Time.Date(GetAttribute("stamp")); }

            set { SetAttribute("stamp", Time.Date(value)); }
        }

        #endregion
    }
}