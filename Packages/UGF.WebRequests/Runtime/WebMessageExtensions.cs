using System;
using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public static class WebMessageExtensions
    {
        public static IList<WebCookie> GetResponseCookies(this IWebMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            return message.Headers.TryGetValue(WebRequestHeaders.SetCookie, out string value) && !string.IsNullOrEmpty(value)
                ? WebRequestUtility.ParseCookieCollection(value)
                : new List<WebCookie>();
        }

        public static void SetResponseCookies(this IWebMessage message, IReadOnlyList<WebCookie> collection)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (collection.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(collection));
            if (message.Headers is not IDictionary<string, string> headers) throw new InvalidOperationException("Can not edit headers.");

            string value = WebRequestUtility.FormatCookieCollection(collection);

            headers[WebRequestHeaders.SetCookie] = value;
        }

        public static IList<(string Name, string Value)> GetRequestCookies(this IWebMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            return message.Headers.TryGetValue(WebRequestHeaders.Cookie, out string value) && !string.IsNullOrEmpty(value)
                ? WebRequestUtility.ParseCookiePairs(value)
                : new List<(string Name, string Value)>();
        }

        public static void SetRequestCookies(this IWebMessage message, IReadOnlyList<(string Name, string Value)> collection)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (collection.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(collection));
            if (message.Headers is not IDictionary<string, string> headers) throw new InvalidOperationException("Can not edit headers.");

            string value = WebRequestUtility.FormatCookiePairs(collection);

            headers[WebRequestHeaders.Cookie] = value;
        }
    }
}
