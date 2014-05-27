// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlRpcException.cs" company="">
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
    public class XmlRpcException : Exception
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public XmlRpcException() {}

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public XmlRpcException(string msg) : base(msg) {}

        /// <summary>
        /// </summary>
        /// <param name="code">
        /// </param>
        /// <param name="msg">
        /// </param>
        public XmlRpcException(int code, string msg) : base(msg)
        {
            Code = code;
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public int Code { get; set; }

        #endregion
    }
}