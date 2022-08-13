using System;
using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public static class WebMessageExtensions
    {
        public static bool TryGetResponseCookies(this IWebMessage message, out List<WebCookie> cookies)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (message.Headers.TryGetValue(WebRequestHeaders.SetCookie, out string value) && !string.IsNullOrEmpty(value))
            {
                cookies = WebRequestUtility.ParseCookieCollection(value);
                return true;
            }

            cookies = default;
            return false;
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

        public static bool TryGetRequestCookies(this IWebMessage message, out List<(string Name, string Value)> cookies)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (message.Headers.TryGetValue(WebRequestHeaders.Cookie, out string value) && !string.IsNullOrEmpty(value))
            {
                cookies = WebRequestUtility.ParseCookiePairs(value);
                return true;
            }

            cookies = default;
            return false;
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
