using System;
using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public class WebRequest : WebMessageBase, IWebRequest
    {
        public WebRequestMethod Method { get; }
        public string Url { get; }

        public WebRequest(WebRequestMethod method, string url) : this(new Dictionary<string, string>(), method, url)
        {
        }

        public WebRequest(Dictionary<string, string> headers, WebRequestMethod method, string url) : base(headers)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("Value cannot be null or empty.", nameof(url));

            Method = method;
            Url = url;
        }
    }
}
