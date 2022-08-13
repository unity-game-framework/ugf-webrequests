using System;
using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public static class WebMessageExtensions
    {
        public static bool TryGetResponseCookies(this IWebMessage message, out WebResponseCookieCollection cookies)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (message.Headers.TryGetValue(WebRequestHeaders.SetCookie, out string value) && !string.IsNullOrEmpty(value))
            {
                cookies = WebResponseCookieCollection.Parse(value);
                return true;
            }

            cookies = default;
            return false;
        }

        public static void SetResponseCookies(this IWebMessage message, IWebCookieCollection<WebResponseCookie> cookies)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));
            if (cookies.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(cookies));
            if (message.Headers is not IDictionary<string, string> headers) throw new InvalidOperationException("Can not edit headers.");

            headers[WebRequestHeaders.SetCookie] = cookies.Format();
        }

        public static bool TryGetRequestCookies(this IWebMessage message, out WebRequestCookieCollection cookies)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (message.Headers.TryGetValue(WebRequestHeaders.Cookie, out string value) && !string.IsNullOrEmpty(value))
            {
                cookies = WebRequestCookieCollection.Parse(value);
                return true;
            }

            cookies = default;
            return false;
        }

        public static void SetRequestCookies(this IWebMessage message, IWebCookieCollection<WebRequestCookie> cookies)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (cookies == null) throw new ArgumentNullException(nameof(cookies));
            if (cookies.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(cookies));
            if (message.Headers is not IDictionary<string, string> headers) throw new InvalidOperationException("Can not edit headers.");

            headers[WebRequestHeaders.Cookie] = cookies.Format();
        }
    }
}
