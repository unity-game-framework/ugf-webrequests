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
