// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDNAException.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region file header


#endregion

namespace agsXMPP.Idn
{
    #region usings

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public class IDNAException : Exception
    {
        #region Members

        /// <summary>
        /// </summary>
        public static string CONTAINS_ACE_PREFIX = "ACE prefix (xn--) not allowed.";

        /// <summary>
        /// </summary>
        public static string CONTAINS_HYPHEN = "Leading or trailing hyphen not allowed.";

        /// <summary>
        /// </summary>
        public static string CONTAINS_NON_LDH = "Contains non-LDH characters.";

        /// <summary>
        /// </summary>
        public static string TOO_LONG = "String too long.";

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="m">
        /// </param>
        public IDNAException(string m) : base(m) {}

        // TODO
        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        public IDNAException(StringprepException e) : base(string.Empty, e) {}

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        public IDNAException(PunycodeException e) : base(string.Empty, e) {}

        #endregion
    }
}