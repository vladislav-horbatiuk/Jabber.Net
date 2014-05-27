// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Admin.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc.iq.admin
{
    #region usings

    using Xml.Dom;

    #endregion

    /*
        <query xmlns='http://jabber.org/protocol/muc#admin'>
            <item nick='pistol' role='none'>
              <reason>Avaunt, you cullion!</reason>
            </item>
        </query>
    */

    /// <summary>
    /// </summary>
    public class Admin : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Admin()
        {
            TagName = "query";
            Namespace = Uri.MUC_ADMIN;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="item">
        /// </param>
        public void AddItem(Item item)
        {
            AddChild(item);
        }

        /// <summary>
        /// </summary>
        /// <param name="items">
        /// </param>
        public void AddItems(Item[] items)
        {
            foreach (Item itm in items)
            {
                AddItem(itm);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public Item[] GetItems()
        {
            ElementList nl = SelectElements(typeof (Item));
            Item[] items = new Item[nl.Count];
            int i = 0;
            foreach (Item itm in nl)
            {
                items[i] = itm;
                i++;
            }

            return items;
        }

        #endregion
    }
}