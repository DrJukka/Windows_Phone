// -----------------------------------------------------------------------
// <copyright file="Response{T}.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Net;
using Nokia.Places.Phone.Types;

namespace Nokia.Places.Phone
{
    /// <summary>
    /// Contains the result or the error if an error occurred.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class Response<T>
    {
        private string _nextUrl;
        private PlaceClient _client;
        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}" /> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="result">The result.</param>
        internal Response(HttpStatusCode? statusCode, T result)
            : this(statusCode, result,null,null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}" /> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="result">The result.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page asked for.</param>
        /// <param name="totalResults">The total results available.</param>
        internal Response(HttpStatusCode? statusCode, T result,string nextUrl,PlaceClient placeClient)
        {
            this.StatusCode = statusCode;
            this.Result = result;
            this._nextUrl = nextUrl;
            this._client = placeClient;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}" /> class.
        /// </summary>
        /// <param name="statusCode">The HTTP Status code</param>
        /// <param name="error">The error.</param>
        internal Response(HttpStatusCode? statusCode, Exception error)
        {
            this.StatusCode = statusCode;
            this.Error = error;
            this._nextUrl = null;
            this._client = null;
        }
        

        /// <summary>
        /// returns true if next page is available
        /// </summary>
        public bool HasNext()
        {
            if (this._nextUrl != null && this._client != null)
            {
                if (this._nextUrl.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }

         /// <summary>
        /// returns true if next page is available
        /// </summary>
        public void GetNextPage(Action<ListResponse<SearchResultItem>> callback)
        {
            if (this.HasNext()){
                this._client.GetUrlSearchResults(callback, this._nextUrl);
            }
        }
 
        /// <summary>
        /// Gets the exception if the call was not successful
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Gets the result if the call was successful
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public T Result { get; private set; }

        /// <summary>
        /// Gets or sets the HTTP Status code
        /// </summary>
        internal HttpStatusCode? StatusCode { get; set; }
    }
}
