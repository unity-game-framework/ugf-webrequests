using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace UGF.WebRequests.Runtime.Tests
{
    public class TestWebResponseCollection
    {
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
        public void Parse()
        {
            string value = string.Join(',', m_cookiesValid.Select(x => x.value));

            WebResponseCookieCollection result = WebResponseCookieCollection.Parse(value);

            for (int i = 0; i < result.Count; i++)
            {
                AssertEqualCookie(m_cookiesValid[i].cookie, result[i]);
            }
        }

        [Test]
        public void Format()
        {
            string value = new WebResponseCookieCollection(m_cookiesValid.Select(x => x.cookie).ToList()).Format();

            WebResponseCookieCollection result = WebResponseCookieCollection.Parse(value);

            for (int i = 0; i < result.Count; i++)
            {
                AssertEqualCookie(m_cookiesValid[i].cookie, result[i]);
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
