using System.Collections.Generic;
using System.Net;

namespace UGF.WebRequests.Runtime
{
    public class WebResponse : WebMessageBase, IWebResponse
    {
        public WebRequestMethod Method { get; }
        public HttpStatusCode StatusCode { get; }

        public WebResponse(WebRequestMethod method, HttpStatusCode statusCode) : this(new Dictionary<string, string>(), method, statusCode)
        {
        }

        public WebResponse(Dictionary<string, string> headers, WebRequestMethod method, HttpStatusCode statusCode) : base(headers)
        {
            Method = method;
            StatusCode = statusCode;
        }
    }
}
