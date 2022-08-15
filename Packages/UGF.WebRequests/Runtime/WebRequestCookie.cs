using System;

namespace UGF.WebRequests.Runtime
{
    public readonly struct WebRequestCookie : IWebCookie
    {
        public string Name { get; }
        public string Value { get; }
        public bool HasValue { get { return !string.IsNullOrEmpty(Value); } }

        public WebRequestCookie(string name, string value = "")
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (!WebRequestUtility.IsValidCookieName(name)) throw new ArgumentException("Name is invalid.");
            if (!string.IsNullOrEmpty(value) && !WebRequestUtility.IsValidCookieValue(value)) throw new ArgumentException("Value is invalid.");

            Name = name;
            Value = value;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name);
        }

        public bool Equals(WebRequestCookie other)
        {
            return Name == other.Name && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is WebRequestCookie other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value);
        }

        public static bool TryParse(string value, out WebRequestCookie cookie)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            try
            {
                cookie = Parse(value);
                return true;
            }
            catch (Exception)
            {
                cookie = default;
                return false;
            }
        }

        public static WebRequestCookie Parse(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            string[] pair = value.Split('=');
            string cookieName = pair[0].Trim();
            string cookieValue = pair.Length > 1 ? pair[1].Trim() : string.Empty;

            return new WebRequestCookie(cookieName, cookieValue);
        }

        public static bool operator ==(WebRequestCookie first, WebRequestCookie second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(WebRequestCookie first, WebRequestCookie second)
        {
            return !first.Equals(second);
        }
    }
}
