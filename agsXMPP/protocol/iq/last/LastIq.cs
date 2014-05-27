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

namespace agsXMPP.protocol.iq.last
{
    using client;

    /// <summary>
    /// Summary description for LastIq.
    /// </summary>
    public class LastIq : IQ
    {
        public LastIq()
        {
            this.GenerateId();
        }

        public LastIq(IqType type)
            : this()
        {
            this.Type = type;
        }

        public LastIq(IqType type, Jid to)
            : this(type)
        {
            this.To = to;
        }

        public LastIq(IqType type, Jid to, Jid from)
            : this(type, to)
        {
            this.From = from;
        }

        public LastIq(IQ iq)
            : base(iq)
        {
        }

        public new Last Query
        {
            get
            {
                return SelectSingleElement(typeof(Last)) as Last;
            }
            set
            {
                RemoveTag(typeof(Last));
                if (value != null)
                {
                    AddChild(value);
                }
            }
        }
    }
}
