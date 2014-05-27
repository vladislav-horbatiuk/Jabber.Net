// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JidException.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region file header


#endregion

#region file header


#endregion

namespace agsXMPP.exceptions
{
    #region usings

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public class JidException : Exception
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public JidException() {}

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public JidException(string msg) : base(msg) {}

        #endregion
    }
}