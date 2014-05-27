// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="DirectionalElement.cs">
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
    /// Base XMPP Element
    /// This must ne used to build all other new packets
    /// </summary>
    public abstract class DirectionalElement : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public DirectionalElement() : base() {}

        /// <summary>
        /// </summary>
        /// <param name="tag">
        /// </param>
        public DirectionalElement(string tag) : base(tag) {}

        /// <summary>
        /// </summary>
        /// <param name="tag">
        /// </param>
        /// <param name="ns">
        /// </param>
        public DirectionalElement(string tag, string ns) : base(tag)
        {
            Namespace = ns;
        }

        /// <summary>
        /// </summary>
        /// <param name="tag">
        /// </param>
        /// <param name="text">
        /// </param>
        /// <param name="ns">
        /// </param>
        public DirectionalElement(string tag, string text, string ns) : base(tag, text)
        {
            Namespace = ns;
        }

        protected DirectionalElement(DirectionalElement e)
            : base(e)
        {
            Switched = e.Switched;
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

            set
            {
                if (value != null)
                {
                    SetAttribute("from", value.ToString());
                }
                else
                {
                    RemoveAttribute("from");
                }
            }
        }

        /// <summary>
        /// </summary>
        public Jid To
        {
            get
            {
                if (HasAttribute("to"))
                {
                    return new Jid(GetAttribute("to"));
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
                    SetAttribute("to", value.ToString());
                }
                else
                {
                    RemoveAttribute("to");
                }
            }
        }

        public bool HasFrom
        {
            get { return From != null && !Equals(From, Jid.Empty); }
        }

        public bool HasTo
        {
            get { return To != null && !Equals(From, Jid.Empty); }
        }

        public bool Switched
		{
			get;
			private set;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Switches the from and to attributes when existing
        /// </summary>
        public void SwitchDirection()
        {
            Jid from = From;
            Jid to = To;

            // Remove from and to now
            RemoveAttribute("from");
            RemoveAttribute("to");

            Jid helper = null;

            helper = from;
            from = to;
            to = helper;

            From = from;
            To = to;

			Switched = !Switched;
        }

        #endregion
    }
}