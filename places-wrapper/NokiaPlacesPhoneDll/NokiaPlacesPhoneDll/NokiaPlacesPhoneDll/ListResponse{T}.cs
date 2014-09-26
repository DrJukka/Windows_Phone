// -----------------------------------------------------------------------
// <copyright file="ListResponse{T}.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;

namespace Nokia.Places.Phone
{
    /// <summary>
    /// Contains the result or the error if an error occurred.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public class ListResponse<T> : Response<List<T>>, IEnumerable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListResponse{T}" /> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="result">The result.</param>
        internal ListResponse(HttpStatusCode? statusCode, List<T> result, string nextUrl, PlaceClient placeClient)
            : base(statusCode, result, nextUrl,placeClient)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListResponse{T}" /> class.
        /// </summary>
        /// <param name="statusCode">The HTTP Status code</param>
        /// <param name="error">The error.</param>
        internal ListResponse(HttpStatusCode? statusCode, Exception error)
            : base(statusCode, error)
        {
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (this.Result != null)
            {
                return this.Result.GetEnumerator();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            if (this.Result != null)
            {
                return this.Result.GetEnumerator();
            }
            else
            {
                return null;
            }
        }
    }
}