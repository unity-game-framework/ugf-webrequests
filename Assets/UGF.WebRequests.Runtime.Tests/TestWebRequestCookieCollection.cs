using System.Collections.Generic;
using NUnit.Framework;

namespace UGF.WebRequests.Runtime.Tests
{
    public class TestWebRequestCookieCollection
    {
        [Test]
        public void Parse()
        {
            var cookies = new List<WebRequestCookie>
            {
                new WebRequestCookie("cookie", "value"),
                new WebRequestCookie("cookie", "value"),
                new WebRequestCookie("empty"),
            };

            WebRequestCookieCollection result = WebRequestCookieCollection.Parse("cookie=value; cookie=value; empty=");

            Assert.AreEqual(cookies.Count, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.True(cookies[i] == result[i]);
            }
        }

        [Test]
        public void Format()
        {
            var cookies = new WebRequestCookieCollection
            {
                new WebRequestCookie("cookie", "value"),
                new WebRequestCookie("cookie", "value"),
                new WebRequestCookie("empty"),
            };

            string result = cookies.Format();

            Assert.AreEqual("cookie=value; cookie=value; empty=", result);
        }
    }
}
