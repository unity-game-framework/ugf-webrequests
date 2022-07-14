using UnityEngine;

namespace UGF.WebRequests.Runtime.Tests
{
    [CreateAssetMenu(menuName = "Tests/TestWebRequestHeaderDropdownAttributeAsset")]
    public class TestWebRequestHeaderDropdownAttributeAsset : ScriptableObject
    {
        [WebRequestHeaderDropdown]
        [SerializeField] private string m_header1;
        [WebRequestHeaderDropdown]
        [SerializeField] private string m_header2;
        [WebRequestHeaderDropdown]
        [SerializeField] private string m_header3;

        public string Header1 { get { return m_header1; } set { m_header1 = value; } }
        public string Header2 { get { return m_header2; } set { m_header2 = value; } }
        public string Header3 { get { return m_header3; } set { m_header3 = value; } }
    }
}
