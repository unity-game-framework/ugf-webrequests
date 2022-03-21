using System;
using UnityEngine;

namespace UGF.WebRequests.Runtime.Http
{
    [CreateAssetMenu(menuName = "Unity Game Framework/Web/Http Web Request Sender", order = 2000)]
    public class HttpWebRequestSenderAsset : WebRequestSenderAsset
    {
        [SerializeField] private bool m_overrideTimeout;
        [SerializeField] private double m_timeout = 100D;
        [SerializeField] private bool m_overrideResponseContentBufferSize;
        [SerializeField] private int m_maxResponseContentBufferSize;

        public bool OverrideTimeout { get { return m_overrideTimeout; } set { m_overrideTimeout = value; } }
        public double Timeout { get { return m_timeout; } set { m_timeout = value; } }
        public bool OverrideResponseContentBufferSize { get { return m_overrideResponseContentBufferSize; } set { m_overrideResponseContentBufferSize = value; } }
        public int MaxResponseContentBufferSize { get { return m_maxResponseContentBufferSize; } set { m_maxResponseContentBufferSize = value; } }

        protected override IWebRequestSender OnBuild()
        {
            var description = new HttpWebRequestSenderDescription
            {
                OverrideTimeout = m_overrideTimeout,
                Timeout = TimeSpan.FromSeconds(m_timeout),
                OverrideMaxResponseContentBufferSize = m_overrideResponseContentBufferSize,
                MaxResponseContentBufferSize = m_maxResponseContentBufferSize
            };

            return new HttpWebRequestSender(description);
        }
    }
}
