using System.Globalization;
using NUnit.Framework;

namespace UGF.WebRequests.Runtime.Tests
{
    public class TestWebRequestsUtility
    {
        [Test]
        public void TryParseCookie()
        {
            bool result1 = WebRequestUtility.TryParseCookie("sessionId=38afes7a8", out WebCookie cookie1);
            bool result2 = WebRequestUtility.TryParseCookie("id=a3fWa; Expires=Wed, 21 Oct 2015 07:28:00 GMT", out WebCookie cookie2);
            bool result3 = WebRequestUtility.TryParseCookie("id=a3fWa; Max-Age=2592000", out WebCookie cookie3);
            bool result4 = WebRequestUtility.TryParseCookie("qwerty=219ffwef9w0f; Domain=somecompany.co.uk", out WebCookie cookie4);

            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);
            Assert.True(result4);
            Assert.AreEqual("sessionId", cookie1.Name);
            Assert.AreEqual("38afes7a8", cookie1.Value);
            Assert.AreEqual("id", cookie2.Name);
            Assert.AreEqual("a3fWa", cookie2.Value);
            Assert.AreEqual("Wed, 21 Oct 2015 07:28:00 GMT", cookie2.Expires.ToString("R"));
            Assert.AreEqual("id", cookie3.Name);
            Assert.AreEqual("2592000", cookie3.MaxAge.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("qwerty", cookie4.Name);
            Assert.AreEqual("219ffwef9w0f", cookie4.Value);
            Assert.AreEqual("somecompany.co.uk", cookie4.Domain);
        }
    }
}
