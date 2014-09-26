// -----------------------------------------------------------------------
// <copyright file="ApiCredentialsRequiredException.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Nokia.Places.Phone
{
    /// <summary>
    /// Exception when no API key has been supplied
    /// </summary>
    public class ApiCredentialsRequiredException : NokiaPlacesException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiCredentialsRequiredException" /> class.
        /// </summary>
        public ApiCredentialsRequiredException() : base("API Credentials AppId and AppCode are required for all method calls")
        {
        }
    }
}
