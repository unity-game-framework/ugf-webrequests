using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace UGF.WebRequests.Runtime.Tests
{
    public class TestWebResponseCookie
    {
        private readonly List<(string value, WebResponseCookie cookie)> m_cookiesFormat = new List<(string value, WebResponseCookie)>
        {
            ("cookie=", new WebResponseCookie("cookie")),
            ("cookie=value", new WebResponseCookie("cookie", "value")),
            ("cookie=value; Expires=Wed, 21 Oct 2015 07:28:00 GMT; Max-Age=556460; Domain=test.com; Path=/path; Secure; HttpOnly; SameSite=Strict", new WebResponseCookie("cookie", "value")
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

        private readonly List<(string value, WebResponseCookie cookie)> m_cookiesValid = new List<(string value, WebResponseCookie)>
        {
            ("cookie", new WebResponseCookie("cookie")),
            ("cookie=", new WebResponseCookie("cookie")),
            ("cookie=value", new WebResponseCookie("cookie", "value")),
            ("cookie=value; Expires=Wed, 21 Oct 2015 07:28:00 GMT; Max-Age=556460; Domain=test.com; Path=/path; Secure; HttpOnly; SameSite=Strict", new WebResponseCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            }),
            ("cookie=value;Expires=Wed, 21 Oct 2015 07:28:00 GMT;Max-Age=556460;Domain=test.com;Path=/path;Secure;HttpOnly;SameSite=Strict", new WebResponseCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            }),
            ("cookie=value; Domain=test.com; Expires=Wed, 21 Oct 2015 07:28:00 GMT; HttpOnly; Max-Age=556460; SameSite=Strict; Path=/path; Secure", new WebResponseCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            }),
            ("cookie=value; expires=Wed, 21 Oct 2015 07:28:00 GMT; max-age=556460; domain=test.com; path=/path; secure; httponly; samesite=Strict", new WebResponseCookie("cookie", "value")
            {
                Expires = DateTimeOffset.Parse("Wed, 21 Oct 2015 07:28:00 GMT"),
                MaxAge = TimeSpan.FromSeconds(556460D),
                Domain = "test.com",
                Path = "/path",
                Secure = true,
                HttpOnly = true,
                SameSite = WebCookieSameSite.Strict
            }),
            ("cookie=value; expires=Wed, 21 Oct 2015 07:28:00 GMT; max-age=556460; domain=test.com; attribute=value; path=/path; secure; httponly; samesite=Strict", new WebResponseCookie("cookie", "value")
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
            foreach ((string value, WebResponseCookie cookie) in m_cookiesValid)
            {
                WebResponseCookie cookieResult = WebResponseCookie.Parse(value);

                AssertEqualCookie(cookie, cookieResult);
            }
        }

        [Test]
        public void ParseCookieInvalid()
        {
            Assert.Throws<ArgumentException>(() => WebResponseCookie.Parse(""));
            Assert.Throws<ArgumentException>(() => WebResponseCookie.Parse(";"));
            Assert.Throws<ArgumentException>(() => WebResponseCookie.Parse(";=value"));
            Assert.Throws<ArgumentException>(() => WebResponseCookie.Parse("cookie@=value"));
        }

        [Test]
        public void TryParseCookie()
        {
            bool result1 = WebResponseCookie.TryParse("sessionId=38afes7a8", out WebResponseCookie cookie1);
            bool result2 = WebResponseCookie.TryParse("id=a3fWa; Expires=Wed, 21 Oct 2015 07:28:00 GMT", out WebResponseCookie cookie2);
            bool result3 = WebResponseCookie.TryParse("id=a3fWa; Max-Age=2592000", out WebResponseCookie cookie3);
            bool result4 = WebResponseCookie.TryParse("qwerty=219ffwef9w0f; Domain=somecompany.co.uk", out WebResponseCookie cookie4);
            bool result5 = WebResponseCookie.TryParse("cook=value; Secure", out WebResponseCookie cookie5);

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
            foreach ((string value, WebResponseCookie cookie) in m_cookiesFormat)
            {
                string result = cookie.Format();

                Assert.AreEqual(result, value);
            }
        }

        private void AssertEqualCookie(WebResponseCookie first, WebResponseCookie second)
        {
            PropertyInfo[] properties = typeof(WebResponseCookie).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Assert.AreEqual(property.GetValue(first), property.GetValue(second), "{0}", property.Name);
            }
        }
    }
}
