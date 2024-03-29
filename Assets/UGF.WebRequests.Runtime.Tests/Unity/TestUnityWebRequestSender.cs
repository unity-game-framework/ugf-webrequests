﻿using System.Collections;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UGF.Initialize.Runtime;
using UGF.WebRequests.Runtime.Unity;
using UnityEngine.TestTools;

namespace UGF.WebRequests.Runtime.Tests.Unity
{
    public class TestUnityWebRequestSender
    {
        [UnityTest]
        public IEnumerator Get()
        {
            var sender = new UnityWebRequestSender(new UnityWebRequestSenderDescription());
            var request = new WebRequest(WebRequestMethod.Get, "https://unitygameframework.jfrog.io/artifactory/api/npm/default/com.ugf.application");

            using (new InitializeScope(sender))
            {
                Task<IWebResponse> task = sender.SendAsync(request);

                while (!task.IsCompleted)
                {
                    yield return null;
                }

                IWebResponse response = task.Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.True(response.HasData, "request.HasData");
                Assert.IsInstanceOf<byte[]>(response.Data);
                Assert.Pass(Encoding.UTF8.GetString((byte[])response.Data));
            }
        }
    }
}
