using System.Threading.Tasks;
using UnityEngine;

namespace UGF.WebRequests.Runtime.Tests
{
    public abstract class TestWebRequestSenderComponentBase : MonoBehaviour
    {
        [SerializeField] private string m_name;
        [SerializeField] private string m_method;
        [SerializeField] private string m_url;
        [SerializeField] private TestLogsComponent m_logs;

        public string Name { get { return m_name; } set { m_name = value; } }
        public string Method { get { return m_method; } set { m_method = value; } }
        public string Url { get { return m_url; } set { m_url = value; } }
        public TestLogsComponent Logs { get { return m_logs; } set { m_logs = value; } }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0F, 0F, Screen.width, Screen.height));

            GUILayout.Label(m_name);

            m_method = GUILayout.TextField(m_method);
            m_url = GUILayout.TextField(m_url);

            if (GUILayout.Button("Send"))
            {
                SendAsync(m_method, m_url);
            }

            m_logs.DrawGUILayout();

            GUILayout.EndArea();
        }

        private async void SendAsync(string methodName, string url)
        {
            WebRequestMethod method = WebRequestUtility.GetMethod(methodName);
            var request = new WebRequest(method, url);

            await OnSendAsync(request);
        }

        protected abstract Task OnSendAsync(IWebRequest request);
    }
}
