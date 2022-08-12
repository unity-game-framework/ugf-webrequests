using System;

namespace UGF.WebRequests.Runtime
{
    public struct WebCookie
    {
        public string Name { get; }
        public string Value { get; }
        public DateTimeOffset Expires { get; set; }
        public TimeSpan MaxAge { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public WebCookieSameSite SameSite { get; set; }

        public WebCookie(string name, string value = "")
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (!WebRequestUtility.IsValidCookieName(name)) throw new ArgumentException("Name is invalid.");
            if (!string.IsNullOrEmpty(value) && !WebRequestUtility.IsValidCookieValue(value)) throw new ArgumentException("Value is invalid.");

            Name = name;
            Value = value;
            Expires = default;
            MaxAge = TimeSpan.Zero;
            Domain = string.Empty;
            Path = string.Empty;
            Secure = false;
            HttpOnly = false;
            SameSite = WebCookieSameSite.None;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
