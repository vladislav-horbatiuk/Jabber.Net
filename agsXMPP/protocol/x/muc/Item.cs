// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Item.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc
{
    /// <summary>
    /// Summary description for Item.
    /// </summary>
    public class Item : Base.Item
    {
        /*
        <x xmlns='http://jabber.org/protocol/muc#user'>
             <item affiliation='admin' role='moderator'/>
        </x>
         
        <item nick='pistol' role='none'>
            <reason>Avaunt, you cullion!</reason>
        </item>
        
        <presence
                from='darkcave@macbeth.shakespeare.lit/thirdwitch'
                to='crone1@shakespeare.lit/desktop'>
                <x xmlns='http://jabber.org/protocol/muc#user'>
                    <item   affiliation='none'
                            jid='hag66@shakespeare.lit/pda'
                            role='participant'/>
                </x>
        </presence>
        */
        #region Constructor

        /// <summary>
        /// </summary>
        public Item() : base()
        {
            TagName = "item";
            Namespace = Uri.MUC_USER;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation">
        /// </param>
        public Item(Affiliation affiliation) : this()
        {
            Affiliation = affiliation;
        }

        /// <summary>
        /// </summary>
        /// <param name="role">
        /// </param>
        public Item(Role role) : this()
        {
            Role = role;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation">
        /// </param>
        /// <param name="role">
        /// </param>
        public Item(Affiliation affiliation, Role role) : this(affiliation)
        {
            Role = role;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation">
        /// </param>
        /// <param name="role">
        /// </param>
        /// <param name="reason">
        /// </param>
        public Item(Affiliation affiliation, Role role, string reason) : this(affiliation, role)
        {
            Reason = reason;
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public Actor Actor
        {
            get { return SelectSingleElement(typeof (Actor)) as Actor; }

            set
            {
                if (HasTag(typeof (Actor)))
                {
                    RemoveTag(typeof (Actor));
                }

                if (value != null)
                {
                    AddChild(value);
                }
            }
        }

        /// <summary>
        /// </summary>
        public Affiliation Affiliation
        {
            get { return (Affiliation) GetAttributeEnum("affiliation", typeof (Affiliation)); }

            set { SetAttribute("affiliation", value.ToString()); }
        }

        /// <summary>
        /// </summary>
        public string Nickname
        {
            get { return GetAttribute("nick"); }

            set { SetAttribute("nick", value); }
        }

        /// <summary>
        /// </summary>
        public string Reason
        {
            get { return GetTag("reason"); }

            set { SetTag("reason", value); }
        }

        /// <summary>
        /// </summary>
        public Role Role
        {
            get { return (Role) GetAttributeEnum("role", typeof (Role)); }

            set { SetAttribute("role", value.ToString()); }
        }

        #endregion
    }
}