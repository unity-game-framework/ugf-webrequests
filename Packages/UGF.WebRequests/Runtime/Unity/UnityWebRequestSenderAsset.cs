using UGF.RuntimeTools.Runtime.Encodings;
using UnityEngine;

namespace UGF.WebRequests.Runtime.Unity
{
    [CreateAssetMenu(menuName = "Unity Game Framework/Web/Unity Web Request Sender", order = 2000)]
    public class UnityWebRequestSenderAsset : WebRequestSenderAsset
    {
        [SerializeField] private int m_redirectLimit = 10;
        [SerializeField] private int m_timeout;
        [SerializeField] private bool m_useHttpContinue = true;
        [SerializeField] private EncodingType m_errorEncoding = EncodingType.Default;

        public int RedirectLimit { get { return m_redirectLimit; } set { m_redirectLimit = value; } }
        public int Timeout { get { return m_timeout; } set { m_timeout = value; } }
        public bool UseHttpContinue { get { return m_useHttpContinue; } set { m_useHttpContinue = value; } }
        public EncodingType ErrorEncoding { get { return m_errorEncoding; } set { m_errorEncoding = value; } }

        protected override IWebRequestSender OnBuild()
        {
            var description = new UnityWebRequestSenderDescription
            {
                RedirectLimit = m_redirectLimit,
                Timeout = m_timeout,
                UseHttpContinue = m_useHttpContinue,
                ErrorEncoding = m_errorEncoding
            };

            return new UnityWebRequestSender(description);
        }
    }
}
