namespace UGF.WebRequests.Runtime
{
    public interface IWebRequest : IWebMessage
    {
        WebRequestMethod Method { get; }
    }
}
