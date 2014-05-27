// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SharpZipBaseException.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.IO.Compression
{
    #region usings

    using System;

    #endregion

    /// <summary>
    /// SharpZipBaseException is the base exception class for the SharpZipLibrary.
    /// All library exceptions are derived from this.
    /// </summary>
    public class SharpZipBaseException : ApplicationException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SharpZipLibraryException class.
        /// </summary>
        public SharpZipBaseException() {}

        /// <summary>
        /// Initializes a new instance of the SharpZipLibraryException class with a specified error message.
        /// </summary>
        /// <param name="msg">
        /// </param>
        public SharpZipBaseException(string msg) : base(msg) {}

        /// <summary>
        /// Initializes a new instance of the SharpZipLibraryException class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// Error message string
        /// </param>
        /// <param name="innerException">
        /// The inner exception
        /// </param>
        public SharpZipBaseException(string message, Exception innerException) : base(message, innerException) {}

        #endregion
    }
}