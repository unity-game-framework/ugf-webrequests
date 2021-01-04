using System;
using System.Collections.Generic;
using System.Net;

namespace UGF.WebRequests.Runtime
{
    public class WebResponse : WebMessageBase, IWebResponse
    {
        public WebRequestMethod Method { get; }
        public string Url { get; }
        public HttpStatusCode StatusCode { get; }

        public WebResponse(WebRequestMethod method, string url, HttpStatusCode statusCode) : this(new Dictionary<string, string>(), method, url, statusCode)
        {
        }

        public WebResponse(Dictionary<string, string> headers, WebRequestMethod method, string url, HttpStatusCode statusCode) : base(headers)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("Value cannot be null or empty.", nameof(url));

            Method = method;
            Url = url;
            StatusCode = statusCode;
        }
    }
}
