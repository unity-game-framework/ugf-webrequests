using System.Collections.Generic;
using UnityEngine;

namespace UGF.WebRequests.Runtime.Http
{
    [CreateAssetMenu(menuName = "Unity Game Framework/Web/Http Web Request Receiver", order = 2000)]
    public class HttpWebRequestReceiverAsset : WebRequestReceiverAsset
    {
        [SerializeField] private List<string> m_prefixes = new List<string>();

        public List<string> Prefixes { get { return m_prefixes; } }

        protected override IWebRequestReceiver OnBuild()
        {
            var description = new HttpWebRequestReceiverDescription();
            IWebRequestReceiveHandler handler = OnBuildHandler();

            for (int i = 0; i < m_prefixes.Count; i++)
            {
                description.Prefixes.Add(m_prefixes[i]);
            }

            return new HttpWebRequestReceiver(description, handler);
        }

        protected virtual IWebRequestReceiveHandler OnBuildHandler()
        {
            return new WebRequestReceiveHandler();
        }
    }
}
