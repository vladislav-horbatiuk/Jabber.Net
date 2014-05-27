
#region Using directives

using System;

#endregion

namespace agsXMPP.protocol.component
{
    using client;

    /// <summary>
    /// Summary description for Presence.
    /// </summary>
    public class Presence : client.Presence
    {
        #region << Constructors >>
        public Presence() : base()
        {
            this.Namespace = Uri.ACCEPT;
        }

        public Presence(ShowType show, string status) : this()
        {
            this.Show = show;
            this.Status = status;
        }

        public Presence(ShowType show, string status, int priority) : this(show, status)
        {
            this.Priority = priority;
        }
        #endregion

        /// <summary>
        /// Error Child Element
        /// </summary>
        public new Error Error
        {
            get
            {
                return SelectSingleElement(typeof(Error)) as Error;

            }
            set
            {
                if (HasTag(typeof(Error)))
                    RemoveTag(typeof(Error));

                if (value != null)
                    this.AddChild(value);
            }
        }
    }
}
