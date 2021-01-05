using UnityEngine;

namespace UGF.WebRequests.Runtime.Tests
{
    public class TestLogsComponent : MonoBehaviour
    {
        private string m_log;
        private Vector2 m_scroll;

        protected virtual void OnEnable()
        {
            Application.logMessageReceived += OnApplicationLogMessageReceived;
        }

        protected virtual void OnDisable()
        {
            Application.logMessageReceived -= OnApplicationLogMessageReceived;
        }

        public void DrawGUILayout()
        {
            m_scroll = GUILayout.BeginScrollView(m_scroll);

            GUILayout.TextArea(m_log, GUILayout.ExpandHeight(true));

            GUILayout.EndScrollView();
        }

        private void OnApplicationLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            m_log += $"\n{type}: {condition}\n{stacktrace}";
        }
    }
}
