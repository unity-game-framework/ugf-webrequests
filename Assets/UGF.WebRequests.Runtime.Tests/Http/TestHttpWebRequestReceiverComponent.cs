using System.Collections.Generic;
using UGF.WebRequests.Runtime.Http;
using UnityEngine;

namespace UGF.WebRequests.Runtime.Tests.Http
{
    public class TestHttpWebRequestReceiverComponent : MonoBehaviour
    {
        [SerializeField] private List<string> m_prefixes = new List<string>();
        [SerializeField] private TestLogsComponent m_logs;

        public List<string> Prefixes { get { return m_prefixes; } }
        public TestLogsComponent Logs { get { return m_logs; } set { m_logs = value; } }

        private HttpWebRequestReceiver m_receiver;

        private void OnEnable()
        {
            var description = new HttpWebRequestReceiverDescription();

            description.Prefixes.AddRange(m_prefixes);

            m_receiver = new HttpWebRequestReceiver(description, new WebRequestReceiveHandler());
            m_receiver.Initialize();
        }

        private void OnDisable()
        {
            m_receiver.Uninitialize();
            m_receiver = null;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0F, 0F, Screen.width, Screen.height));

            m_logs.DrawGUILayout();

            GUILayout.EndArea();
        }
    }
}
