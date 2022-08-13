using System;
using System.Text;
using System.Text.RegularExpressions;

namespace UGF.WebRequests.Runtime
{
    public class WebResponseCookieCollection : WebCookieCollection<WebResponseCookie>
    {
        public static bool TryParse(string value, out WebResponseCookieCollection cookies)
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

        public static WebResponseCookieCollection Parse(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            var cookies = new WebResponseCookieCollection();

            value = new Regex("(?<=expires=).{29}", RegexOptions.IgnoreCase).Replace(value, match => DateTimeOffset.Parse(match.Value).ToString("O"));

            string[] values = value.TrimEnd(';').Split(',');

            foreach (string cookieValue in values)
            {
                WebResponseCookie cookie = WebResponseCookie.Parse(cookieValue.Trim());

                cookies.Add(cookie);
            }

            return cookies;
        }

        protected override string OnFormat()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < Count; i++)
            {
                WebResponseCookie cookie = this[i];

                if (!cookie.IsValid()) throw new ArgumentException("Value should be valid.", nameof(cookie));

                string value = cookie.Format();

                builder.Append(value);

                if (i < Count - 1)
                {
                    builder.Append(',');
                    builder.Append(' ');
                }
            }

            return builder.ToString();
        }
    }
}
