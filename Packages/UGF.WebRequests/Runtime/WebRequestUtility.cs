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
                {
                    throw new ArgumentOutOfRangeException(nameof(method), method, "Unknown method specified.");
                }
            }
        }

        public static WebRequestMethod GetMethod(string method)
        {
            switch (method)
            {
                case "CREATE": return WebRequestMethod.Create;
                case "DELETE": return WebRequestMethod.Delete;
                case "GET": return WebRequestMethod.Get;
                case "HEAD": return WebRequestMethod.Head;
                case "POST": return WebRequestMethod.Post;
                case "PUT": return WebRequestMethod.Put;
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(method), method, "Unknown method name specified.");
                }
            }
        }

        public static bool TryParseCookie(string value, out WebCookie cookie)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            string[] parts = value.Trim().Split(';');
            string[] nameAndValue = parts[0].Trim().Split('=');

            if (nameAndValue.Length > 0 && !string.IsNullOrEmpty(nameAndValue[0]))
            {
                cookie = nameAndValue.Length > 1
                    ? new WebCookie(nameAndValue[0].Trim(), nameAndValue[1].Trim())
                    : new WebCookie(nameAndValue[0].Trim());

                foreach (string part in parts[1..])
                {
                    string[] attributes = part.Split('=');

                    if (attributes.Length > 1)
                    {
                        string attributeName = attributes[0].Trim().ToLowerInvariant();
                        string attributeValue = attributes[1].Trim();

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
                                if (TimeSpan.TryParse(attributeValue, out TimeSpan maxAge))
                                {
                                    cookie.MaxAge = maxAge;
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
                                if (bool.TryParse(attributeValue, out bool secure))
                                {
                                    cookie.Secure = secure;
                                }

                                break;
                            }
                            case "httponly":
                            {
                                if (bool.TryParse(attributeValue, out bool httpOnly))
                                {
                                    cookie.HttpOnly = httpOnly;
                                }

                                break;
                            }
                            case "SameSite":
                            {
                                if (Enum.TryParse(attributeValue, out WebCookieSameSite sameSite))
                                {
                                    cookie.SameSite = sameSite;
                                }

                                break;
                            }
                        }
                    }
                }

                return true;
            }

            cookie = default;
            return false;
        }

        public static bool IsValidCookieName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            foreach (char character in name)
            {
                switch (character)
                {
                    case ' ':
                    case '(':
                    case ')':
                    case '<':
                    case '>':
                    case '@':
                    case ',':
                    case ';':
                    case ':':
                    case '\\':
                    case '"':
                    case '/':
                    case '[':
                    case ']':
                    case '?':
                    case '=':
                    case '{':
                    case '}':
                    {
                        return false;
                    }
                }

                if (char.IsControl(character) || char.IsWhiteSpace(character))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsValidCookieValue(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            foreach (char character in value)
            {
                switch (character)
                {
                    case ' ':
                    case ',':
                    case ';':
                    case '\\':
                    case '"':
                    {
                        return false;
                    }
                }

                if (char.IsControl(character) || char.IsWhiteSpace(character))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
