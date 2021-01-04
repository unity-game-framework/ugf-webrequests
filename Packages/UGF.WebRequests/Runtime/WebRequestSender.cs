using System;
using System.Threading.Tasks;
using UGF.Description.Runtime;
using UGF.Initialize.Runtime;

namespace UGF.WebRequests.Runtime
{
    public abstract class WebRequestSender<TDescription> : Initializable, IWebRequestSender, IDescribed<TDescription> where TDescription : class, IWebRequestSenderDescription
    {
        public TDescription Description { get; }

        protected WebRequestSender(TDescription description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public Task<IWebResponse> SendAsync(IWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return OnSendAsync(request);
        }

        public T GetDescription<T>() where T : class, IDescription
        {
            return (T)GetDescription();
        }

        public IDescription GetDescription()
        {
            return Description;
        }

        protected abstract Task<IWebResponse> OnSendAsync(IWebRequest request);
    }
}
