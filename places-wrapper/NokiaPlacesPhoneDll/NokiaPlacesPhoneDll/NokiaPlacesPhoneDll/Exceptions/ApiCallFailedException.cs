
// -----------------------------------------------------------------------
// <copyright file="ApiCallFailedException.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Nokia.Places.Phone
{
    /// <summary>
    /// Exception when an API call fails unexpectedly
    /// </summary>
    public class ApiCallFailedException : NokiaPlacesException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiCallFailedException"/> class.
        /// </summary>
        public ApiCallFailedException()
            : base("Unexpected failure, check connectivity")
        {
        }
    }
}