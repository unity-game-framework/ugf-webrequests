using System.Net;

namespace UGF.WebRequests.Runtime
{
    public interface IWebResponse : IWebMessage
    {
        WebRequestMethod Method { get; }
        HttpStatusCode StatusCode { get; }
    }
}
