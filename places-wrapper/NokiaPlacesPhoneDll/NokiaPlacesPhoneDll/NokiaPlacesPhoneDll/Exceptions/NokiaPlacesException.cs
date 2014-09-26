// -----------------------------------------------------------------------
// <copyright file="NokiaPlacesException.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Nokia.Places.Phone
{
    /// <summary>
    /// Generic Nokia Music Exception.
    /// </summary>
    /// 
    public class  NokiaPlacesException: Exception
    {
 
        /// <summary>
        /// Initializes a new instance of the <see cref="NokiaPlacesException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public NokiaPlacesException(string message)
            : base(message)
        {
        }  
    }
}
