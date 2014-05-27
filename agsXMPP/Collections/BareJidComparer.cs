// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BareJidComparer.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region file header


#endregion

namespace agsXMPP.Collections
{
    #region usings

    using System;
    using System.Collections;

    #endregion

    /// <summary>
    /// </summary>
    public class BareJidComparer : IComparer
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <returns>
        /// </returns>
        public int Compare(object x, object y)
        {
            if (x is Jid && y is Jid)
            {
                Jid jidX = (Jid) x;
                Jid jidY = (Jid) y;

                if (jidX.Bare == jidY.Bare)
                {
                    return 0;
                }
                else
                {
                    return String.Compare(jidX.Bare, jidY.Bare);
                }
            }

            throw new ArgumentException("the objects to compare must be Jids");
        }

        #endregion
    }
}