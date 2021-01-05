using System.Collections;
using UGF.Initialize.Runtime;
using UGF.WebRequests.Runtime.Http;
using UnityEngine;
using UnityEngine.TestTools;

namespace UGF.WebRequests.Runtime.Tests.Http
{
    public class TestHttpWebRequestReceiver
    {
        [UnityTest]
        public IEnumerator InitializeAndUninitialize()
        {
            var description = new HttpWebRequestReceiverDescription
            {
                Prefixes =
                {
                    "http://*:8080/"
                }
            };

            var receiver = new HttpWebRequestReceiver(description, new WebRequestReceiveHandler());

            using (new InitializeScope(receiver))
            {
                yield return new WaitForSeconds(1F);
            }
        }
    }
}
