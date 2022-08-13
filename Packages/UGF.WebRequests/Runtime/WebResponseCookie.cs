using System;
using System.Text;

namespace UGF.WebRequests.Runtime
{
    public struct WebResponseCookie : IWebCookie
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

        public WebResponseCookie(string name, string value = "")
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (!WebRequestCookieUtility.IsValidCookieName(name)) throw new ArgumentException("Name is invalid.");
            if (!string.IsNullOrEmpty(value) && !WebRequestCookieUtility.IsValidCookieValue(value)) throw new ArgumentException("Value is invalid.");

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

        public string Format()
        {
            var builder = new StringBuilder();

            builder.Append(Name);
            builder.Append('=');

            if (HasValue)
            {
                builder.Append(Value);
            }

            if (HasExpires)
            {
                AddAttribute(builder, "Expires", Expires.ToString("R"));
            }

            if (HasMaxAge)
            {
                AddAttribute(builder, "Max-Age", MaxAge.TotalSeconds.ToString("#0"));
            }

            if (HasDomain)
            {
                AddAttribute(builder, "Domain", Domain);
            }

            if (HasPath)
            {
                AddAttribute(builder, "Path", Path);
            }

            if (Secure)
            {
                AddAttribute(builder, "Secure");
            }

            if (HttpOnly)
            {
                AddAttribute(builder, "HttpOnly");
            }

            if (HasSameSite)
            {
                AddAttribute(builder, "SameSite", SameSite.ToString());
            }

            static void AddAttribute(StringBuilder builder, string name, string value = "")
            {
                builder.Append(';').Append(' ');
                builder.Append(name);

                if (!string.IsNullOrEmpty(value))
                {
                    builder.Append('=');
                    builder.Append(value);
                }
            }

            return builder.ToString();
        }

        public bool Equals(WebResponseCookie other)
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
            return obj is WebResponseCookie other && Equals(other);
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

        public static bool TryParse(string value, out WebResponseCookie cookie)
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

        public static WebResponseCookie Parse(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            string[] parts = value.TrimEnd(';').Split(';');
            string[] nameAndValue = parts[0].Trim().Split('=');

            if (nameAndValue.Length <= 0 || string.IsNullOrEmpty(nameAndValue[0])) throw new ArgumentException("Name not found.");

            WebResponseCookie cookie = nameAndValue.Length > 1
                ? new WebResponseCookie(nameAndValue[0].Trim(), nameAndValue[1].Trim())
                : new WebResponseCookie(nameAndValue[0].Trim());

            foreach (string part in parts[1..])
            {
                string[] attributes = part.Split('=');

                if (attributes.Length > 0)
                {
                    string attributeName = attributes[0].Trim().ToLowerInvariant();
                    string attributeValue = attributes.Length > 1 ? attributes[1].Trim() : string.Empty;

                    switch (attributeName)
                    {
                        case "expires":
                        {
                            if (DateTimeOffset.TryParse(attributeValue, out DateTimeOffset expires))
                            {
                                cookie.Expires = expires;
                            }

                            break;
                        }
                        case "max-age":
                        {
                            if (double.TryParse(attributeValue, out double maxAge))
                            {
                                cookie.MaxAge = TimeSpan.FromSeconds(maxAge);
                            }

                            break;
                        }
                        case "domain":
                        {
                            cookie.Domain = attributeValue;
                            break;
                        }
                        case "path":
                        {
                            cookie.Path = attributeValue;
                            break;
                        }
                        case "secure":
                        {
                            cookie.Secure = true;
                            break;
                        }
                        case "httponly":
                        {
                            cookie.HttpOnly = true;
                            break;
                        }
                        case "samesite":
                        {
                            if (Enum.TryParse(attributeValue, true, out WebCookieSameSite sameSite))
                            {
                                cookie.SameSite = sameSite;
                            }

                            break;
                        }
                    }
                }
            }

            return cookie;
        }

        public static bool operator ==(WebResponseCookie first, WebResponseCookie second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(WebResponseCookie first, WebResponseCookie second)
        {
            return !first.Equals(second);
        }
    }
}
