// -----------------------------------------------------------------------
// <copyright file="ApiUriBuilder.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Nokia.Places.Phone.Internal
{
    /// <summary>
    /// Defines the real Api Uri Builder
    /// </summary>
    internal sealed class ApiUriBuilder : IApiUriBuilder
    {
        /// <summary>
        /// Builds an API URI
        /// </summary>
        /// <param name="method">The method to call.</param>
        /// <param name="appId">The App ID obtained from api.developer.nokia.com</param>
        /// <param name="appCode">The App Code obtained from api.developer.nokia.com</param>
        /// <param name="pathParams">The path parameters.</param>
        /// <param name="querystringParams">The querystring parameters.</param>
        /// <returns>
        /// A Uri to call
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when an unknown method is used</exception>
        /// <exception cref="ApiCredentialsRequiredException">Thrown when an API Key has not been supplied</exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        public Uri BuildUri(ApiMethod method, string appId, string appCode, Dictionary<string, string> querystringParams, Dictionary<string, string> pathParams)
        {
            // Validate Method and Country Code if it's required
            switch (method)
            {
                case ApiMethod.Search:
                case ApiMethod.Explore:
                case ApiMethod.DiscoverHere:
                case ApiMethod.GetFullPlace:
                case ApiMethod.GetSuggestions:
                case ApiMethod.GetCategories:
                    break;
                case ApiMethod.GetUrlResults:
                    {
                        // no need for any parameters, so we can return already
                        return new Uri(pathParams["url"]);
                    } 
                case ApiMethod.Unknown:
                default:
                    throw new ArgumentOutOfRangeException("method");
                    break;
            }

            // API Key
            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appCode))
            {
                throw new ApiCredentialsRequiredException();
            }

            // Build API url
            StringBuilder url = new StringBuilder();
            url.Append(@"http://places.nlp.nokia.com/places/v1/");

            switch (method)
            {
                case ApiMethod.Search:
                    url.Append("discover/search");
                    break;
                case ApiMethod.Explore:
                    url.Append("discover/explore");
                    break;
                case ApiMethod.DiscoverHere:
                    url.Append("discover/here");
                    break;
                case ApiMethod.GetFullPlace:
                    url.AppendFormat("places/{0}", pathParams["placeId"]);
                    break;
                case ApiMethod.GetSuggestions:
                    url.Append("suggest");
                    break;
                case ApiMethod.GetCategories:
                    url.Append("categories/places");
                    break;      
            }

            // Add required parameters...
            url.AppendFormat(@"?app_id={0}", appId);
            url.AppendFormat(@"&app_code={0}", appCode);
            

            // Add other parameters...
            if (querystringParams != null)
            {
                foreach (string key in querystringParams.Keys)
                {
                    url.AppendFormat(@"&{0}={1}", key, querystringParams[key]);
                }
            }

            return new Uri(url.ToString());
        }
    }
}
