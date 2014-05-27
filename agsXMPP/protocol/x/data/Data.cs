// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Data.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.data
{
    #region usings

    using Xml.Dom;

    #endregion

    /// <summary>
    /// Form Types
    /// </summary>
    public enum XDataFormType
    {
        /// <summary>
        /// The forms-processing entity is asking the forms-submitting entity to complete a form.
        /// </summary>
        form, 

        /// <summary>
        /// The forms-submitting entity is submitting data to the forms-processing entity.
        /// </summary>
        submit, 

        /// <summary>
        /// The forms-submitting entity has cancelled submission of data to the forms-processing entity.
        /// </summary>
        cancel, 

        /// <summary>
        /// The forms-processing entity is returning data (e.g., search results) to the forms-submitting entity, or the data is a generic data set.
        /// </summary>
        result
    }

    /// <summary>
    /// Summary for Data.
    /// </summary>
    public class Data : FieldContainer
    {
        /*
		The base syntax for the 'jabber:x:data' namespace is as follows (a formal description can be found in the XML Schema section below):
		
		<x xmlns='jabber:x:data'
		type='{form-type}'>
		<title/>
		<instructions/>
		<field var='field-name'
				type='{field-type}'
				label='description'>
			<desc/>
			<required/>
			<value>field-value</value>
			<option label='option-label'><value>option-value</value></option>
			<option label='option-label'><value>option-value</value></option>
		</field>
		</x>
		
		*/
        #region Constructor

        /// <summary>
        /// </summary>
        public Data()
        {
            TagName = "x";
            Namespace = Uri.X_DATA;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public Data(XDataFormType type) : this()
        {
            Type = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public string Instructions
        {
            get { return GetTag("instructions"); }

            set { SetTag("instructions", value); }
        }

        /// <summary>
        /// </summary>
        public Reported Reported
        {
            get { return SelectSingleElement(typeof (Reported)) as Reported; }

            set
            {
                RemoveTag(typeof (Reported));
                AddChild(value);
            }
        }

        /// <summary>
        /// </summary>
        public string Title
        {
            get { return GetTag("title"); }

            set { SetTag("title", value); }
        }

        /// <summary>
        /// Type of thie XDATA Form.
        /// </summary>
        public XDataFormType Type
        {
            get { return (XDataFormType) GetAttributeEnum("type", typeof (XDataFormType)); }

            set { SetAttribute("type", value.ToString()); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public Item AddItem()
        {
            Item i = new Item();
            AddChild(i);
            return i;
        }

        /// <summary>
        /// </summary>
        /// <param name="item">
        /// </param>
        /// <returns>
        /// </returns>
        public Item AddItem(Item item)
        {
            AddChild(item);
            return item;
        }

        /// <summary>
        /// Gets a list of all form fields
        /// </summary>
        /// <returns>
        /// </returns>
        public Item[] GetItems()
        {
            ElementList nl = SelectElements(typeof (Item));
            Item[] items = new Item[nl.Count];
            int i = 0;
            foreach (Element e in nl)
            {
                items[i] = (Item) e;
                i++;
            }

            return items;
        }

        #endregion
    }
}