// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterException.cs" company="">
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
    public class RegisterException : Exception
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public RegisterException() {}

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public RegisterException(string msg) : base(msg) {}

        #endregion
    }
}