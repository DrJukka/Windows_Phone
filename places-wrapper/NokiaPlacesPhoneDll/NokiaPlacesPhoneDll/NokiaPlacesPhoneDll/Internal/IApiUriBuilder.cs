// -----------------------------------------------------------------------
// <copyright file="IApiUriBuilder.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Nokia.Places.Phone.Internal
{
    /// <summary>
    /// Defines API methods available to call
    /// </summary>
    internal enum ApiMethod
    {
        /// <summary>
        /// Unknown API
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Search API
        /// </summary>
        Search = 1,

        /// <summary>
        /// Explore popular places for a location
        /// </summary>
        Explore = 2,

        /// <summary>
        /// Discover Here for places within specific location context
        /// </summary>
        DiscoverHere = 3,
        /// <summary>
        /// get place with full information
        /// </summary>
        GetFullPlace = 4,
        /// <summary>
        /// get suggestion list for search
        /// </summary>
        GetSuggestions = 5,
        /// <summary>
        /// get local categories
        /// </summary>
        GetCategories = 6,
        /// <summary>
        /// Just forward the Url
        /// </summary>
        GetUrlResults = 7
    }

    /// <summary>
    /// Defines the API URI Builder interface
    /// </summary>
    internal interface IApiUriBuilder
    {
        /// <summary>
        /// Builds an API URI
        /// </summary>
        /// <param name="method">The method to call.</param>
        /// <param name="appId">The App ID obtained from api.developer.nokia.com</param>
        /// <param name="appCode">The App Code obtained from api.developer.nokia.com</param>
        /// <param name="querystringParams">The querystring parameters.</param>
        /// <returns>A Uri to call</returns>
        Uri BuildUri(ApiMethod method, string appId, string appCode, Dictionary<string, string> querystringParams, Dictionary<string, string> pathParams);
    }
}
