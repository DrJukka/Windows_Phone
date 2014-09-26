// -----------------------------------------------------------------------
// <copyright file="ApiRequestHandler.cs" company="Nokia">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Nokia.Places.Phone.Internal
{
    /// <summary>
    /// Implementation of the raw API interface for making requests
    /// </summary>
    internal class ApiRequestHandler : IApiRequestHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestHandler" /> class.
        /// </summary>
        /// <param name="uriBuilder">The URI builder.</param>
        public ApiRequestHandler(IApiUriBuilder uriBuilder)
        {
            this.UriBuilder = uriBuilder;
        }

        /// <summary>
        /// Gets the URI builder that is being used.
        /// </summary>
        /// <value>
        /// The URI builder.
        /// </value>
        public IApiUriBuilder UriBuilder { get; private set; }

        /// <summary>
        /// Makes the API request
        /// </summary>
        /// <param name="method">The method to call.</param>
        /// <param name="appId">The App ID obtained from api.developer.nokia.com</param>
        /// <param name="appCode">The App Code obtained from api.developer.nokia.com</param>
        /// <param name="pathParams">The path params.</param>
        /// <param name="querystringParams">The querystring params.</param>
        /// <param name="callback">The callback to hit when done.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when no callback is specified</exception>
        public void SendRequestAsync(ApiMethod method, string appId, string appCode, Dictionary<string, string> querystringParams, Dictionary<string, string> pathParams, Action<Response<JObject>> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            Uri uri = this.UriBuilder.BuildUri(method, appId, appCode, querystringParams, pathParams);

            Debug.WriteLine("Calling " + uri.ToString());

            WebRequest request = WebRequest.Create(uri);
            request.BeginGetResponse(
                (IAsyncResult ar) =>
                {
                    WebResponse response = null;
                    HttpWebResponse webResponse = null;
                    JObject json = null;
                    HttpStatusCode? statusCode = null;
                    Exception error = null;

                    try
                    {
                        response = request.EndGetResponse(ar);
                        webResponse = response as HttpWebResponse;
                        if (webResponse != null)
                        {
                            statusCode = webResponse.StatusCode;
                        }
                    }
                    catch (WebException ex)
                    {
                        error = ex;
                        if (ex.Response != null)
                        {
                            webResponse = (HttpWebResponse)ex.Response;
                            statusCode = webResponse.StatusCode;
                        }
                    }

                    string result = null;
                    if (response != null)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                result = reader.ReadToEnd();
                                if (!string.IsNullOrEmpty(result))
                                {
                                    try
                                    {
                                        json = JObject.Parse(result);
                                    }
                                    catch (Exception ex)
                                    {
                                        error = ex;
                                        json = null;
                                    }
                                }
                            }
                        }
                    }

                    if (json != null)
                    {
                        callback(new Response<JObject>(statusCode, json));
                    }
                    else
                    {
                        callback(new Response<JObject>(statusCode, error));
                    }
                },
                request);
        }
    }
}
