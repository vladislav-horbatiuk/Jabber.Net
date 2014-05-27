// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="FieldContainer.cs">
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
    /// Bass class for all xdata classes that contain xData fields
    /// </summary>
    public abstract class FieldContainer : Element
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public Field AddField()
        {
            Field f = new Field();
            AddChild(f);
            return f;
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        public Field AddField(Field field)
        {
            AddChild(field);
            return field;
        }

        /// <summary>
        /// Retrieve a field with the given "var"
        /// </summary>
        /// <param name="var">
        /// </param>
        /// <returns>
        /// </returns>
        public Field GetField(string var)
        {
            ElementList nl = SelectElements(typeof (Field));
            foreach (Element e in nl)
            {
                Field f = e as Field;
                if (f.Var == var)
                {
                    return f;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a list of all form fields
        /// </summary>
        /// <returns>
        /// </returns>
        public Field[] GetFields()
        {
            ElementList nl = SelectElements(typeof (Field));
            Field[] fields = new Field[nl.Count];
            int i = 0;
            foreach (Element e in nl)
            {
                fields[i] = (Field) e;
                i++;
            }

            return fields;
        }

        #endregion
    }
}