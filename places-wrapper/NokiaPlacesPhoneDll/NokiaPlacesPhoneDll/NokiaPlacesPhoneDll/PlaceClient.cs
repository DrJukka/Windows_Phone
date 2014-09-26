// -----------------------------------------------------------------------
// <copyright file="PlaceClient.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Device.Location;

using Nokia.Places.Phone.Internal;
using Nokia.Places.Phone.Types;

namespace Nokia.Places.Phone
{
    /// <summary>
    /// The Nokia Place API client
    /// </summary>
    public class PlaceClient : IPlaceClient
    {
        private string _appId;
        private string _appCode;
        private IApiRequestHandler _requestHandler;
        

        private const string ParamSearchTerm = "q";
        private const string ParamLocationTerm = "at";
        
        private const string ParamAreaTerm1 = "in";
        private const string ParamAreaTerm2 = ";r=";

        private const string ParamCategoryTerm = "cat";

        private const string ParamSizeTerm = "size";
        private const string ParamOffsetTerm = "offset";


        private const string ArrayNameItems = "items";

        private const string PlaceIdParametrer = "placeId";

        private const string UrlParametrer = "url";
        

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceClient" /> class,
        /// </summary>
        /// <param name="appId">The App ID obtained from api.developer.nokia.com</param>
        /// <param name="appCode">The App Code obtained from api.developer.nokia.com</param>
        public PlaceClient(string appId, string appCode)
             : this(appId, appCode, new ApiRequestHandler(new ApiUriBuilder()))
        {
      
        }

         /// <summary>
        /// Initializes a new instance of the <see cref="PlaceClient" /> class.
        /// </summary>
        /// <param name="appId">The App ID obtained from api.developer.nokia.com</param>
        /// <param name="appCode">The App Code obtained from api.developer.nokia.com</param>
        /// <param name="requestHandler">The request handler.</param>
        /// <remarks>
        /// Allows custom requestHandler for testing purposes
        /// </remarks>
        internal PlaceClient(string appId, string appCode, IApiRequestHandler requestHandler)
        {
            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appCode))
            {
                throw new ApiCredentialsRequiredException();
            }

            this._appId = appId;
            this._appCode = appCode;
            this._requestHandler = requestHandler;
        }

        /// <summary>
        /// Signifies a method for converting a JToken into a typed object
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>A typed object</returns>
        private delegate T JTokenConversionDelegate<T>(JToken item);

        /// <summary>
        /// Gets the request handler.
        /// </summary>
        /// <value>
        /// The request handler.
        /// </value>
        internal IApiRequestHandler RequestHandler
        {
            get
            {
                return this._requestHandler;
            }
        }

        /// <summary>
        /// Searches for an Place
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        public void Search(Action<ListResponse<SearchResultItem>> callback, GeoCoordinate location, string searchTerm, int size, int offset)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentNullException("A searchTerm must be supplied", "searchTerm");
            }

            if (location == null)
            {
                throw new ArgumentNullException("A location for the search must be supplied", "location");
            }

            this.ValidateCallback(callback);
            this.InternalSearch<SearchResultItem>(searchTerm, location, size, offset, SearchResultItem.FromJToken, callback);
        }

      
        /// <summary>
        /// Explore for an Place's nearby
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="categories">comma separated list of categories.</param> 
        /// <param name="maxDistance">Readius in meters to which the results are explored for.</param> 
        /// <param name="callback">The callback to use when the API call has completed</param>
        public void Explore(Action<ListResponse<SearchResultItem>> callback, GeoCoordinate location, string categories, int maxDistance, int size, int offset)
        {
            if (location == null)
            {
                throw new ArgumentNullException("A location for the search must be supplied", "location");
            }

            this.ValidateCallback(callback);
            this.InternalExplore<SearchResultItem>(categories, location, maxDistance, size, offset, SearchResultItem.FromJToken, callback);
        }


        /// <summary>
        /// Discovers Place's within specific location context
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="categories">comma separated list of categories.</param> 
        /// <param name="callback">The callback to use when the API call has completed</param>
        public void DiscoverHere(Action<ListResponse<SearchResultItem>> callback, GeoCoordinate location, string categories, int size, int offset)
        {
            if (location == null)
            {
                throw new ArgumentNullException("A location for the search must be supplied", "location");
            }

            this.ValidateCallback(callback);
            this.InternalDiscoverHere<SearchResultItem>(categories, location, size, offset, SearchResultItem.FromJToken, callback);
        }

        /// <summary>
        /// Discovers Place's within specific location context
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="categories">comma separated list of categories.</param> 
        /// <param name="callback">The callback to use when the API call has completed</param>
        public void GetPlace(Action<Response<PlaceItem>> callback, string placeId)
        {
            if (string.IsNullOrEmpty(placeId))
            {
                throw new ArgumentNullException("A placeId must be supplied", "placeId");
            }

            this.ValidateCallback(callback);
            this.InternalGetPlace<PlaceItem>(placeId, PlaceItem.FromJToken, callback);
        }
     

        /// <summary>
        /// Get Suggestions for the search
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="location">The location .</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        public void GetSuggestions(Action<Response<SuggestionList>> callback, string searchTerm, GeoCoordinate location)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentNullException("A searchTerm must be supplied", "searchTerm");
            }

            if (location == null)
            {
                throw new ArgumentNullException("A location for the search must be supplied", "location");
            }

            this.ValidateCallback(callback);
            this.InternalSuggestions<SuggestionList>(searchTerm, location, SuggestionList.FromJToken, callback);
        }
        
        /// <local categoriessummary>
        /// Get list of Local categories
        /// </summary>
        //// <param name="location">The location .</param>
        /// <param name="maxDistance">Readius in meters to which the results are explored for.</param> 
        /// <param name="callback">The callback to use when the API call has completed</param>
        public void GetLocalCategories(Action<ListResponse<CategoryItem>> callback, GeoCoordinate location, int maxDistance = -1)
        {
            if (location == null)
            {
                throw new ArgumentNullException("A location for the search must be supplied", "location");
            }

            this.ValidateCallback(callback);
            this.InternalGetCategories<CategoryItem>(maxDistance, location, CategoryItem.FromJToken, callback);
        }


        /// <summary>
        /// Gets search results for the full Url ifetched earlier with other searches
        /// </summary>
        /// <param name="url">Full Url retrieved with earlier results.</param>>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        public void GetUrlSearchResults(Action<ListResponse<SearchResultItem>> callback, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("A url must be supplied", "url");
            }

            this.ValidateCallback(callback);
            this.InternalGetUrl<SearchResultItem>(url, SearchResultItem.FromJToken, callback);
        }


        /// <summary>
        /// Checks that a callback has been set
        /// </summary>
        /// <param name="callback">The callback</param>
        private void ValidateCallback(object callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("A callback must be supplied", "callback");
            }
        }
      
   /// <summary>
        /// discover here  places 
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="categories">categories for the search.</param>
        /// <param name="location">The location to explore.</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        private void InternalDiscoverHere<T>(string categories, GeoCoordinate location, int size, int offset, JTokenConversionDelegate<T> converter, Action<ListResponse<T>> callback)
        {
            this.ValidateCallback(callback);

            // Build querystring parameters...
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (location != null)
            {
                parameters.Add(ParamLocationTerm, location.Latitude + "," + location.Longitude);
            }


            if (!string.IsNullOrEmpty(categories))
            {
                parameters.Add(ParamCategoryTerm, categories);
            }

            if (size > 0)
            {
                parameters.Add(ParamSizeTerm, size.ToString());
            }

            if (offset > 0)
            {
                parameters.Add(ParamOffsetTerm, offset.ToString());
            }

            this.RequestHandler.SendRequestAsync(
                ApiMethod.DiscoverHere,
                this._appId,
                this._appCode,
                parameters,
                null,
                (Response<JObject> rawResult) =>
                {
                    this.SearchItemResponseHandler<T>(rawResult, ArrayNameItems, converter, callback);
                });
        }
         

        /// <summary>
        /// explore  places nearby 
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="categories">categories for the search.</param>
        /// <param name="location">The location to explore.</param>
        /// <param name="maxDistance">maximun distance to explore.</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        private void InternalExplore<T>(string categories, GeoCoordinate location, int maxDistance, int size, int offset, JTokenConversionDelegate<T> converter, Action<ListResponse<T>> callback)
        {
            this.ValidateCallback(callback);

            // Build querystring parameters...
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (location != null)
            {
                if (maxDistance > 0)
                {
                    parameters.Add(ParamAreaTerm1, location.Latitude + "," + location.Longitude + ParamAreaTerm2 + maxDistance);
                }
                else
                {
                    parameters.Add(ParamLocationTerm, location.Latitude + "," + location.Longitude);
                }
            }


            if (!string.IsNullOrEmpty(categories))
            {
                parameters.Add(ParamCategoryTerm, categories);
            }

            if (size > 0)
            {
                parameters.Add(ParamSizeTerm, size.ToString());
            }

            if (offset > 0)
            {
                parameters.Add(ParamOffsetTerm, offset.ToString());
            }

            this.RequestHandler.SendRequestAsync(
                ApiMethod.Explore,
                this._appId,
                this._appCode,
                parameters,
                null,
                (Response<JObject> rawResult) =>
                {
                    this.SearchItemResponseHandler<T>(rawResult, ArrayNameItems, converter, callback);
                });
        }

        /// <summary>
        /// Searches for an place
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="location">The location .</param>
        /// <param name="category">The cwhere to search to .</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        private void InternalSearch<T>(string searchTerm, GeoCoordinate location, int size, int offset, JTokenConversionDelegate<T> converter, Action<ListResponse<T>> callback)
        {
            this.ValidateCallback(callback);

            // Build querystring parameters...
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (location != null)
            {
                parameters.Add(ParamLocationTerm, location.Latitude + "," + location.Longitude);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                parameters.Add(ParamSearchTerm, searchTerm);
            }

            if (size > 0){
                parameters.Add(ParamSizeTerm, size.ToString());
            }

            if (offset > 0){
                parameters.Add(ParamOffsetTerm, offset.ToString());
            }

            this.RequestHandler.SendRequestAsync(
                ApiMethod.Search,
                this._appId,
                this._appCode,
                parameters,
                null,
                (Response<JObject> rawResult) =>
                {
                    this.SearchItemResponseHandler<T>(rawResult, ArrayNameItems, converter, callback);
                });
        }

        /// <summary>
        /// Get full detailed place item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="PlaceId">The id for the place.</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        private void InternalGetPlace<T>(string PlaceId, JTokenConversionDelegate<T> converter, Action<Response<T>> callback)
        {
            this.ValidateCallback(callback);

            // Build querystring parameters...
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();


            if (!string.IsNullOrEmpty(PlaceId))
            {
                pathParameters.Add(PlaceIdParametrer, PlaceId);
            }

            this.RequestHandler.SendRequestAsync(
                ApiMethod.GetFullPlace,
                this._appId,
                this._appCode,
                null,
                pathParameters,
                (Response<JObject> rawResult) =>
                {
                    this.OneItemResponseHandler<T>(rawResult, ArrayNameItems, converter, callback);
                });
        }

        /// <summary>
        /// Get Suggestions for the search
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="location">The location .</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        private void InternalSuggestions<T>(string searchTerm, GeoCoordinate location, JTokenConversionDelegate<T> converter, Action<Response<T>> callback)
        {
            this.ValidateCallback(callback);

            // Build querystring parameters...
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (location != null)
            {
                parameters.Add(ParamLocationTerm, location.Latitude + "," + location.Longitude);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                parameters.Add(ParamSearchTerm, searchTerm);
            }

            this.RequestHandler.SendRequestAsync(
                ApiMethod.GetSuggestions,
                this._appId,
                this._appCode,
                parameters,
                null,
                (Response<JObject> rawResult) =>
                {
                    this.OneItemResponseHandler<T>(rawResult, ArrayNameItems, converter, callback);
                });
        }

        /// <summary>
        /// Get local categories
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="location">The location to explore.</param>
        /// <param name="maxDistance">maximun distance to explore.</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        private void InternalGetCategories<T>(int maxDistance, GeoCoordinate location, JTokenConversionDelegate<T> converter, Action<ListResponse<T>> callback)
        {
            this.ValidateCallback(callback);

            // Build querystring parameters...
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (location != null)
            {
                if (maxDistance > 0)
                {
                    parameters.Add(ParamAreaTerm1, location.Latitude + "," + location.Longitude + ParamAreaTerm2 + maxDistance);
                }
                else
                {
                    parameters.Add(ParamLocationTerm, location.Latitude + "," + location.Longitude);
                }
            }

            this.RequestHandler.SendRequestAsync(
                ApiMethod.GetCategories,
                this._appId,
                this._appCode,
                parameters,
                null,
                (Response<JObject> rawResult) =>
                {
                    this.CategoryItemResponseHandler<T>(rawResult, ArrayNameItems, converter, callback);
                });
        }


        /// <summary>
        /// Gets search results for the full Url ifetched earlier with other searches
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="url">Full Url retrieved with earlier results.</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The callback to use when the API call has completed</param>
        /// 
        private void InternalGetUrl<T>(string url, JTokenConversionDelegate<T> converter, Action<ListResponse<T>> callback)
        {
            this.ValidateCallback(callback);

            // Build querystring parameters...
            Dictionary<string, string> pathParameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(url))
            {
                pathParameters.Add(UrlParametrer, url);
            }

            this.RequestHandler.SendRequestAsync(
                ApiMethod.GetUrlResults,
                this._appId,
                this._appCode,
                null,
                pathParameters,
                (Response<JObject> rawResult) =>
                {
                    this.SearchItemResponseHandler<T>(rawResult, ArrayNameItems, converter, callback);
                });
        }

         /// <summary>
        /// Generic response handler for content lists
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="rawResult">The response</param>
        /// <param name="itemsName">The json list name</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The client callback</param>
        private void SearchItemResponseHandler<T>(Response<JObject> rawResult, string itemsName, JTokenConversionDelegate<T> converter, Action<ListResponse<T>> callback)
        {
            ListResponse<T> response = null;
            string next_url = null;

            // Parse the result if we got one...
            if (rawResult.StatusCode != null && rawResult.StatusCode.HasValue)
            {
                switch (rawResult.StatusCode.Value)
                {
                    case HttpStatusCode.OK:
                        List<T> results = new List<T>();

                        JToken jsonRes = rawResult.Result.Value<JToken>("results");
                        if (jsonRes != null){
                            // Url for next results page
                            next_url = jsonRes.Value<string>("next");

                            foreach (JToken item in jsonRes.Value<JArray>(itemsName))
                            {
                                T result = converter(item);
                                if (result != null)
                                {
                                    results.Add(result);
                                }
                            }
                        }else{
                            // Url for next results page
                            next_url = rawResult.Result.Value<string>("next");

                            foreach (JToken item in rawResult.Result.Value<JArray>(itemsName))
                            {
                                T result = converter(item);
                                if (result != null)
                                {
                                    results.Add(result);
                                }
                            }
                        }

                        response = new ListResponse<T>(rawResult.StatusCode, results, next_url, this);
                        break;

                    case HttpStatusCode.NotFound:
                        break;
                }
            }

            if (response == null)
            {
                response = new ListResponse<T>(rawResult.StatusCode, new ApiCallFailedException());
            }

            callback(response);
        }

        

        /// <summary>
        ///  response handler for place request
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="rawResult">The response</param>
        /// <param name="itemsName">The json list name</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The client callback</param>
        private void OneItemResponseHandler<T>(Response<JObject> rawResult, string itemsName, JTokenConversionDelegate<T> converter, Action<Response<T>> callback)
        {
            Response<T> response = null;

            // Parse the result if we got one...
            if (rawResult.StatusCode != null && rawResult.StatusCode.HasValue)
            {
                switch (rawResult.StatusCode.Value)
                {
                    case HttpStatusCode.OK:
                        {
                            T result = converter(rawResult.Result);
                            if (result != null)
                            {
                                response = new Response<T>(rawResult.StatusCode, result);
                            }
                        }
                        break;
                    case HttpStatusCode.NotFound:
                        break;
                }
            }

            if (response == null)
            {
                response = new Response<T>(rawResult.StatusCode, new ApiCallFailedException());
            }

            callback(response);
        }


        /// <summary>
        /// Response handler for category list
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="rawResult">The response</param>
        /// <param name="itemsName">The json list name</param>
        /// <param name="converter">The object creation method to use</param>
        /// <param name="callback">The client callback</param>
        private void CategoryItemResponseHandler<T>(Response<JObject> rawResult, string itemsName, JTokenConversionDelegate<T> converter, Action<ListResponse<T>> callback)
        {
            ListResponse<T> response = null;

            // Parse the result if we got one...
            if (rawResult.StatusCode != null && rawResult.StatusCode.HasValue)
            {
                switch (rawResult.StatusCode.Value)
                {
                    case HttpStatusCode.OK:
                        List<T> results = new List<T>();

                        foreach (JToken item in rawResult.Result.Value<JArray>(itemsName))
                        {
                            T result = converter(item);
                            if (result != null)
                            {
                                results.Add(result);
                            }
                        }

                        response = new ListResponse<T>(rawResult.StatusCode, results,null,null);
                        break;

                    case HttpStatusCode.NotFound:
                        break;
                }
            }

            if (response == null)
            {
                response = new ListResponse<T>(rawResult.StatusCode, new ApiCallFailedException());
            }

            callback(response);
        }
    }
}
