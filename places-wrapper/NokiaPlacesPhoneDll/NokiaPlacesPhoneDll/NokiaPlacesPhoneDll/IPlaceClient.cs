// -----------------------------------------------------------------------
// <copyright file="IPlaceClient.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Device.Location;
using System.Collections.Generic;
using Nokia.Places.Phone.Types;

namespace Nokia.Places.Phone
{
    /// <summary>
    /// Defines the Nokia Place API
    /// </summary>
    public interface IPlaceClient
    {
        /// <summary>
        /// Searches for an Place
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="location">The search location.</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        void Search(Action<ListResponse<SearchResultItem>> callback, GeoCoordinate location, string searchTerm, int size = -1, int offset = -1);

        

        /// <summary>
        /// Searches for an Place's nearby
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="categories">comma separated list of categories.</param> 
        /// <param name="callback">The callback to use when the API call has completed</param>
        void Explore(Action<ListResponse<SearchResultItem>> callback, GeoCoordinate location, string categories = "", int maxDistance = -1, int size = -1, int offset = -1);


        /// <summary>
        /// Discovers Place's within specific location context
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="categories">comma separated list of categories.</param> 
        /// <param name="callback">The callback to use when the API call has completed</param>
        void DiscoverHere(Action<ListResponse<SearchResultItem>> callback, GeoCoordinate location, string categories="", int size = -1, int offset = -1);

        /// <summary>
        /// Discovers Place's within specific location context
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="categories">comma separated list of categories.</param> 
        /// <param name="callback">The callback to use when the API call has completed</param>
        void GetPlace(Action<Response<PlaceItem>> callback, string placeId);


        /// <summary>
        /// Get Suggestions for the search
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="location">The location .</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        void GetSuggestions(Action<Response<SuggestionList>> callback, string searchTerm, GeoCoordinate location);        

        /// <local categoriessummary>
        /// Get list of Local categories
        /// </summary>
        //// <param name="location">The location .</param>
        /// <param name="maxDistance">Readius in meters to which the results are explored for.</param> 
        /// <param name="callback">The callback to use when the API call has completed</param>
        void GetLocalCategories(Action<ListResponse<CategoryItem>> callback, GeoCoordinate location, int maxDistance=-1);
        
        /// <summary>
        /// Gets search results for the full Url ifetched earlier with other searches
        /// </summary>
        /// <param name="url">Full Url retrieved with earlier results.</param>>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        void GetUrlSearchResults(Action<ListResponse<SearchResultItem>> callback, string url);
    }
}
