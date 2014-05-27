/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2008 by AG-Software 											 *
 * All Rights Reserved.																 *
 * Contact information for AG-Software is available at http://www.ag-software.de	 *
 *																					 *
 * Licence:																			 *
 * The agsXMPP SDK is released under a dual licence									 *
 * agsXMPP can be used under either of two licences									 *
 * 																					 *
 * A commercial licence which is probably the most appropriate for commercial 		 *
 * corporate use and closed source projects. 										 *
 *																					 *
 * The GNU Public License (GPL) is probably most appropriate for inclusion in		 *
 * other open source projects.														 *
 *																					 *
 * See README.html for details.														 *
 *																					 *
 * For general enquiries visit our website at:										 *
 * http://www.ag-software.de														 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using agsXMPP.protocol.client;

namespace agsXMPP.protocol.iq.vcard
{
    using client;

    //<iq id="id_62" to="gnauck@myjabber.net" type="get"><vCard xmlns="vcard-temp"/></iq>

    /// <summary>
    /// Summary description for VcardIq.
    /// </summary>
    public class VcardIq : IQ
    {
        private Vcard m_Vcard = new Vcard();

        #region << Constructors >>

        public VcardIq(IQ iq)
            : base(iq)
        {
        }

        public VcardIq()
        {
            this.GenerateId();
            this.AddChild(m_Vcard);
        }

        public VcardIq(IqType type)
            : this()
        {
            this.Type = type;
        }

        public VcardIq(IqType type, Vcard vcard)
            : this(type)
        {
            this.Vcard = vcard;
        }

        public VcardIq(IqType type, Jid to)
            : this(type)
        {
            this.To = to;
        }

        public VcardIq(IqType type, Jid to, Vcard vcard)
            : this(type, to)
        {
            this.Vcard = vcard;
        }

        public VcardIq(IqType type, Jid to, Jid from)
            : this(type, to)
        {
            this.From = from;
        }

        public VcardIq(IqType type, Jid to, Jid from, Vcard vcard)
            : this(type, to, from)
        {
            this.Vcard = vcard;
        }
        #endregion


        /// <summary>
        /// Get or Set the VCard if it is a Vcard IQ
        /// </summary>
        public Vcard Vcard
        {
            get { return SelectSingleElement("vCard") as Vcard; }

            set
            {
                if (value != null)
                {
                    ReplaceChild(value);
                }
                else
                {
                    RemoveTag("vCard");
                }
            }
        }
    }
}
