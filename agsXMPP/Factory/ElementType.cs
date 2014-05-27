// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementType.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region file header


#endregion

namespace agsXMPP.Factory
{
    /// <summary>
    /// </summary>
    public class ElementType
    {
        #region Members

        /// <summary>
        /// </summary>
        private readonly string m_Namespace;

        /// <summary>
        /// </summary>
        private readonly string m_TagName;

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="TagName">
        /// </param>
        /// <param name="Namespace">
        /// </param>
        public ElementType(string TagName, string Namespace)
        {
            m_TagName = TagName;
            m_Namespace = Namespace;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            if ((m_Namespace != null) && (m_Namespace != string.Empty))
            {
                return m_Namespace + ":" + m_TagName;
            }

            return m_TagName;
        }

        #endregion
    }
}