using System;

namespace UGF.WebRequests.Runtime
{
    public struct WebCookie
    {
        public string Name { get; }
        public string Value { get; }
        public bool HasValue { get { return !string.IsNullOrEmpty(Value); } }
        public DateTimeOffset Expires { get; set; }
        public bool HasExpires { get { return Expires != default; } }
        public TimeSpan MaxAge { get; set; }
        public bool HasMaxAge { get { return MaxAge != default; } }
        public string Domain { get; set; }
        public bool HasDomain { get { return !string.IsNullOrEmpty(Domain); } }
        public string Path { get; set; }
        public bool HasPath { get { return !string.IsNullOrEmpty(Path); } }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public WebCookieSameSite SameSite { get; set; }
        public bool HasSameSite { get { return SameSite != WebCookieSameSite.None; } }

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

        public bool Equals(WebCookie other)
        {
            return Name == other.Name
                   && Value == other.Value
                   && Expires.Equals(other.Expires)
                   && MaxAge.Equals(other.MaxAge)
                   && Domain == other.Domain
                   && Path == other.Path
                   && Secure == other.Secure
                   && HttpOnly == other.HttpOnly
                   && SameSite == other.SameSite;
        }

        public override bool Equals(object obj)
        {
            return obj is WebCookie other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            hashCode.Add(Name);
            hashCode.Add(Value);
            hashCode.Add(Expires);
            hashCode.Add(MaxAge);
            hashCode.Add(Domain);
            hashCode.Add(Path);
            hashCode.Add(Secure);
            hashCode.Add(HttpOnly);
            hashCode.Add((int)SameSite);

            return hashCode.ToHashCode();
        }

        public static bool operator ==(WebCookie first, WebCookie second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(WebCookie first, WebCookie second)
        {
            return !first.Equals(second);
        }
    }
}
