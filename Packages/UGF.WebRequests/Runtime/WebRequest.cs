using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public class WebRequest : WebMessageBase, IWebRequest
    {
        public WebRequestMethod Method { get; }

        public WebRequest(WebRequestMethod method) : this(new Dictionary<string, string>(), method)
        {
        }

        public WebRequest(Dictionary<string, string> headers, WebRequestMethod method) : base(headers)
        {
            Method = method;
        }
    }
}
