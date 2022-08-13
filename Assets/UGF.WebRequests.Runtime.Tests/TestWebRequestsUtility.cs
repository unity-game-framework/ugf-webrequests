using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace UGF.WebRequests.Runtime.Tests
{
    public class TestWebRequestsUtility
    {
        private readonly List<(string value, WebCookie)> m_cookiesFormat = new List<(string value, WebCookie)>
        {
            ("cookie=", new WebCookie("cookie")),
            ("cookie=value", new WebCookie("cookie", "value")),
            ("cookie=value; Expires=Wed, 21 Oct 2015 07:28:00 GMT; Max-Age=556460; Domain=test.com; Path=/path; Secure; HttpOnly; SameSite=Strict", new WebCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            })
        };

        private readonly List<(string value, WebCookie)> m_cookiesValid = new List<(string value, WebCookie)>
        {
            ("cookie", new WebCookie("cookie")),
            ("cookie=", new WebCookie("cookie")),
            ("cookie=value", new WebCookie("cookie", "value")),
            ("cookie=value; Expires=Wed, 21 Oct 2015 07:28:00 GMT; Max-Age=556460; Domain=test.com; Path=/path; Secure; HttpOnly; SameSite=Strict", new WebCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            }),
            ("cookie=value;Expires=Wed, 21 Oct 2015 07:28:00 GMT;Max-Age=556460;Domain=test.com;Path=/path;Secure;HttpOnly;SameSite=Strict", new WebCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            }),
            ("cookie=value; Domain=test.com; Expires=Wed, 21 Oct 2015 07:28:00 GMT; HttpOnly; Max-Age=556460; SameSite=Strict; Path=/path; Secure", new WebCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            }),
            ("cookie=value; expires=Wed, 21 Oct 2015 07:28:00 GMT; max-age=556460; domain=test.com; path=/path; secure; httponly; samesite=Strict", new WebCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            }),
            ("cookie=value; expires=Wed, 21 Oct 2015 07:28:00 GMT; max-age=556460; domain=test.com; attribute=value; path=/path; secure; httponly; samesite=Strict", new WebCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            })
        };

        [Test]
        public void ParseCookieValid()
        {
            foreach ((string value, WebCookie cookie) in m_cookiesValid)
            {
                WebCookie cookieResult = WebRequestUtility.ParseCookie(value);

                AssertEqualCookie(cookie, cookieResult);
            }
        }

        [Test]
        public void ParseCookieInvalid()
        {
            Assert.Throws<ArgumentException>(() => WebRequestUtility.ParseCookie(""));
            Assert.Throws<ArgumentException>(() => WebRequestUtility.ParseCookie(";"));
            Assert.Throws<ArgumentException>(() => WebRequestUtility.ParseCookie(";=value"));
            Assert.Throws<ArgumentException>(() => WebRequestUtility.ParseCookie("cookie@=value"));
        }

        [Test]
        public void TryParseCookie()
        {
            bool result1 = WebRequestUtility.TryParseCookie("sessionId=38afes7a8", out WebCookie cookie1);
            bool result2 = WebRequestUtility.TryParseCookie("id=a3fWa; Expires=Wed, 21 Oct 2015 07:28:00 GMT", out WebCookie cookie2);
            bool result3 = WebRequestUtility.TryParseCookie("id=a3fWa; Max-Age=2592000", out WebCookie cookie3);
            bool result4 = WebRequestUtility.TryParseCookie("qwerty=219ffwef9w0f; Domain=somecompany.co.uk", out WebCookie cookie4);
            bool result5 = WebRequestUtility.TryParseCookie("cook=value; Secure", out WebCookie cookie5);

            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);
            Assert.True(result4);
            Assert.True(result5);
            Assert.AreEqual("sessionId", cookie1.Name);
            Assert.AreEqual("38afes7a8", cookie1.Value);
            Assert.AreEqual("id", cookie2.Name);
            Assert.AreEqual("a3fWa", cookie2.Value);
            Assert.AreEqual("Wed, 21 Oct 2015 07:28:00 GMT", cookie2.Expires.ToString("R"));
            Assert.AreEqual("id", cookie3.Name);
            Assert.AreEqual(2592000D, cookie3.MaxAge.TotalSeconds);
            Assert.AreEqual("qwerty", cookie4.Name);
            Assert.AreEqual("219ffwef9w0f", cookie4.Value);
            Assert.AreEqual("somecompany.co.uk", cookie4.Domain);
            Assert.AreEqual("cook", cookie5.Name);
            Assert.AreEqual("value", cookie5.Value);
            Assert.True(cookie5.Secure);
        }

        [Test]
        public void FormatCookie()
        {
            foreach ((string value, WebCookie cookie) in m_cookiesFormat)
            {
                string result = WebRequestUtility.FormatCookie(cookie);

                Assert.AreEqual(result, value);
            }
        }

        private void AssertEqualCookie(WebCookie first, WebCookie second)
        {
            PropertyInfo[] properties = typeof(WebCookie).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Assert.AreEqual(property.GetValue(first), property.GetValue(second), "{0}", property.Name);
            }
        }
    }
}
