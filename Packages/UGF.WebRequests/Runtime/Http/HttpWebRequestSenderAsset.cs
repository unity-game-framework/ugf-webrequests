using System;
using UnityEngine;

namespace UGF.WebRequests.Runtime.Http
{
    [CreateAssetMenu(menuName = "Unity Game Framework/Web/Http Web Request Sender", order = 2000)]
    public class HttpWebRequestSenderAsset : WebRequestSenderAsset
    {
        [SerializeField] private double m_timeout = 100D;
        [SerializeField] private long m_maxResponseContentBufferSize = -1;

        public double Timeout { get { return m_timeout; } set { m_timeout = value; } }
        public long MaxResponseContentBufferSize { get { return m_maxResponseContentBufferSize; } set { m_maxResponseContentBufferSize = value; } }

        protected override IWebRequestSender OnBuild()
        {
            var description = new HttpWebRequestSenderDescription
            {
                Timeout = TimeSpan.FromSeconds(m_timeout),
                MaxResponseContentBufferSize = m_maxResponseContentBufferSize >= 0 ? m_maxResponseContentBufferSize : long.MaxValue
            };

            return new HttpWebRequestSender(description);
        }
    }
}
