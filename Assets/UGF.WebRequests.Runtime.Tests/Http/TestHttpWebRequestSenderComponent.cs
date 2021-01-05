using System.Threading.Tasks;
using UGF.WebRequests.Runtime.Http;

namespace UGF.WebRequests.Runtime.Tests.Http
{
    public class TestHttpWebRequestSenderComponent : TestWebRequestSenderComponentBase
    {
        private HttpWebRequestSender m_sender;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_sender = new HttpWebRequestSender(new HttpWebRequestSenderDescription());
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
