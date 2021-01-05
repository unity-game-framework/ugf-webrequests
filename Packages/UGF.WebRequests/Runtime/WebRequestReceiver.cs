using System;
using System.Threading.Tasks;
using UGF.Description.Runtime;
using UGF.Initialize.Runtime;

namespace UGF.WebRequests.Runtime
{
    public abstract class WebRequestReceiver<TDescription> : Initializable, IWebRequestReceiver, IDescribed<TDescription> where TDescription : class, IWebRequestReceiverDescription
    {
        public TDescription Description { get; }
        public IWebRequestReceiveHandler Handler { get; }

        protected WebRequestReceiver(TDescription description, IWebRequestReceiveHandler handler)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public T GetDescription<T>() where T : class, IDescription
        {
            return (T)GetDescription();
        }

        public IDescription GetDescription()
        {
            return Description;
        }

        protected async Task<IWebResponse> HandleRequestAsync(IWebRequest request)
        {
            IWebResponse response = await Handler.ExecuteAsync(this, request);

            if (response == null) throw new ArgumentNullException(nameof(response), "Value cannot be null or empty.");

            return response;
        }
    }
}
