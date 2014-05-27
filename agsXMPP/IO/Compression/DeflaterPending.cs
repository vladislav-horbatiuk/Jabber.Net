// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="DeflaterPending.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.IO.Compression
{
    /// <summary>
    /// This class stores the pending output of the Deflater.
    /// 
    /// author of the original java version : Jochen Hoenicke
    /// </summary>
    public class DeflaterPending : PendingBuffer
    {
        #region Constructor

        /// <summary>
        /// Construct instance with default buffer size
        /// </summary>
        public DeflaterPending() : base(DeflaterConstants.PENDING_BUF_SIZE) {}

        #endregion
    }
}