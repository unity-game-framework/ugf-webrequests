﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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

        public static List<WebCookie> ParseCookieCollection(string value)
        {
            var result = new List<WebCookie>();

            ParseCookieCollection(value, result);

            return result;
        }

        public static void ParseCookieCollection(string value, ICollection<WebCookie> result)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            value = new Regex("(?<=expires=).{29}", RegexOptions.IgnoreCase).Replace(value, match => DateTimeOffset.Parse(match.Value).ToString("O"));

            string[] values = value.TrimEnd(';').Split(',');

            foreach (string cookieValue in values)
            {
                WebCookie cookie = ParseCookie(cookieValue.Trim());

                result.Add(cookie);
            }
        }

        public static bool TryParseCookie(string value, out WebCookie cookie)
        {
            try
            {
                cookie = ParseCookie(value);
                return true;
            }
            catch (Exception)
            {
                cookie = default;
                return false;
            }
        }

        public static WebCookie ParseCookie(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            string[] parts = value.TrimEnd(';').Split(';');
            string[] nameAndValue = parts[0].Trim().Split('=');

            if (nameAndValue.Length <= 0 || string.IsNullOrEmpty(nameAndValue[0])) throw new ArgumentException("Name not found.");

            WebCookie cookie = nameAndValue.Length > 1
                ? new WebCookie(nameAndValue[0].Trim(), nameAndValue[1].Trim())
                : new WebCookie(nameAndValue[0].Trim());

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

        public static List<(string Name, string Value)> ParseCookiePairs(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            var result = new List<(string Name, string Value)>();

            ParseCookiePairs(value, result);

            return result;
        }

        public static void ParseCookiePairs(string value, ICollection<(string Name, string Value)> result)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));
            if (result == null) throw new ArgumentNullException(nameof(result));

            string[] parts = value.TrimEnd(';').Split(';');

            foreach (string part in parts)
            {
                string[] pair = part.Split('=');
                string cookieName = pair[0].Trim();
                string cookieValue = pair.Length > 1 ? pair[1].Trim() : string.Empty;

                result.Add((cookieName, cookieValue));
            }
        }

        public static string FormatCookieCollection(IReadOnlyList<WebCookie> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (collection.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(collection));

            var builder = new StringBuilder();

            for (int i = 0; i < collection.Count; i++)
            {
                WebCookie cookie = collection[i];

                if (!cookie.IsValid()) throw new ArgumentException("Value should be valid.", nameof(cookie));

                string value = FormatCookie(cookie);

                builder.Append(value);

                if (i < collection.Count - 1)
                {
                    builder.Append(',');
                    builder.Append(' ');
                }
            }

            return builder.ToString();
        }

        public static string FormatCookie(WebCookie cookie)
        {
            if (!cookie.IsValid()) throw new ArgumentException("Value should be valid.", nameof(cookie));

            var builder = new StringBuilder();

            builder.Append(cookie.Name);
            builder.Append('=');

            if (cookie.HasValue)
            {
                builder.Append(cookie.Value);
            }

            if (cookie.HasExpires)
            {
                AddAttribute(builder, "Expires", cookie.Expires.ToString("R"));
            }

            if (cookie.HasMaxAge)
            {
                AddAttribute(builder, "Max-Age", cookie.MaxAge.TotalSeconds.ToString("#0"));
            }

            if (cookie.HasDomain)
            {
                AddAttribute(builder, "Domain", cookie.Domain);
            }

            if (cookie.HasPath)
            {
                AddAttribute(builder, "Path", cookie.Path);
            }

            if (cookie.Secure)
            {
                AddAttribute(builder, "Secure");
            }

            if (cookie.HttpOnly)
            {
                AddAttribute(builder, "HttpOnly");
            }

            if (cookie.HasSameSite)
            {
                AddAttribute(builder, "SameSite", cookie.SameSite.ToString());
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
