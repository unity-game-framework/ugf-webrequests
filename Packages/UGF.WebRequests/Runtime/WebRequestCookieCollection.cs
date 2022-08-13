using System;
using System.Text;

namespace UGF.WebRequests.Runtime
{
    public class WebRequestCookieCollection : WebCookieCollection<WebRequestCookie>
    {
        public static bool TryParse(string value, out WebRequestCookieCollection cookies)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            try
            {
                cookies = Parse(value);
                return true;
            }
            catch (Exception)
            {
                cookies = default;
                return false;
            }
        }

        public static WebRequestCookieCollection Parse(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            var cookies = new WebRequestCookieCollection();
            string[] parts = value.TrimEnd(';').Split(';');

            foreach (string part in parts)
            {
                string[] pair = part.Split('=');
                string cookieName = pair[0].Trim();
                string cookieValue = pair.Length > 1 ? pair[1].Trim() : string.Empty;

                cookies.Add(new WebRequestCookie(cookieName, cookieValue));
            }

            return cookies;
        }

        protected override string OnFormat()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < Count; i++)
            {
                WebRequestCookie cookie = this[i];

                builder.Append(cookie.Name);
                builder.Append('=');
                builder.Append(cookie.Value);

                if (i < Count - 1)
                {
                    builder.Append(';');
                    builder.Append(' ');
                }
            }

            return builder.ToString();
        }
    }
}
