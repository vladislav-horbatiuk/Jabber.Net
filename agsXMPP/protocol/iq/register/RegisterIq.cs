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

namespace agsXMPP.protocol.iq.register
{
    using client;

    /// <summary>
    /// Used for registering new usernames on Jabber/XMPP Servers
    /// </summary>
    public class RegisterIq : IQ
    {
        public RegisterIq()
        {
            this.GenerateId();
        }

        public RegisterIq(IQ iq)
            : base(iq)
        {
        }

        public new Register Query
        {
            get
            {
                return SelectSingleElement(typeof(Register)) as Register;
            }
            set
            {
                RemoveTag(typeof(Register));
                if (value != null)
                {
                    AddChild(value);
                }
            }
        }
    }
}