using System;
using System.Threading.Tasks;
using UGF.Description.Runtime;

namespace UGF.WebRequests.Runtime
{
    public abstract class WebRequestSender<TDescription> : DescribedBase<TDescription>, IWebRequestSender where TDescription : class, IWebRequestSenderDescription
    {
        protected WebRequestSender(TDescription description) : base(description)
        {
        }

        public Task<IWebResponse> SendAsync(IWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return OnSendAsync(request);
        }

        protected abstract Task<IWebResponse> OnSendAsync(IWebRequest request);
    }
}
