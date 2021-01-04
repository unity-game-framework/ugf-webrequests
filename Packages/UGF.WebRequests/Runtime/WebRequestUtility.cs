using System;

namespace UGF.WebRequests.Runtime
{
    public static class WebRequestUtility
    {
        public static string GetMethodName(WebRequestMethod method)
        {
            switch (method)
            {
                case WebRequestMethod.Create: return "CREATE";
                case WebRequestMethod.Delete: return "DELETE";
                case WebRequestMethod.Get: return "GET";
                case WebRequestMethod.Head: return "HEAD";
                case WebRequestMethod.Post: return "POST";
                case WebRequestMethod.Put: return "PUT";
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }
    }
}
