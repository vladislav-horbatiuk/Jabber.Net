// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Value.cs">
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
    /// Summary description for Value.
    /// </summary>
    public class Value : Element
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Value()
        {
            TagName = "value";
            Namespace = Uri.X_DATA;
        }

        /// <summary>
        /// </summary>
        /// <param name="val">
        /// </param>
        public Value(string val) : this()
        {
            Value = val;
        }

        /// <summary>
        /// </summary>
        /// <param name="val">
        /// </param>
        public Value(bool val) : this()
        {
            Value = val ? "1" : "0";
        }

        #endregion
    }
}