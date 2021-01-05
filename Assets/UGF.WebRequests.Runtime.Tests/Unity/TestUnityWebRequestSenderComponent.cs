using System.Threading.Tasks;
using UGF.WebRequests.Runtime.Unity;

namespace UGF.WebRequests.Runtime.Tests.Unity
{
    public class TestUnityWebRequestSenderComponent : TestWebRequestSenderComponentBase
    {
        private UnityWebRequestSender m_sender;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_sender = new UnityWebRequestSender(new UnityWebRequestSenderDescription());
            m_sender.Initialize();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_sender.Uninitialize();
            m_sender = null;
        }

        protected override async Task OnSendAsync(IWebRequest request)
        {
            await m_sender.SendAsync(request);
        }
    }
}
