// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="StringprepException.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.Idn
{
    #region usings

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public class StringprepException : Exception
    {
        #region Members

        /// <summary>
        /// </summary>
        public static string BIDI_BOTHRAL = "Contains both R and AL code points.";

        /// <summary>
        /// </summary>
        public static string BIDI_LTRAL = "Leading and trailing code points not both R or AL.";

        /// <summary>
        /// </summary>
        public static string CONTAINS_PROHIBITED = "Contains prohibited code points.";

        /// <summary>
        /// </summary>
        public static string CONTAINS_UNASSIGNED = "Contains unassigned code points.";

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        public StringprepException(string message) : base(message) {}

        #endregion
    }
}