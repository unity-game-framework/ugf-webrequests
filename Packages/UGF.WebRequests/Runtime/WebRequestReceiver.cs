using System;
using UGF.Description.Runtime;
using UGF.Initialize.Runtime;

namespace UGF.WebRequests.Runtime
{
    public abstract class WebRequestReceiver<TDescription> : Initializable, IWebRequestReceiver, IDescribed<TDescription> where TDescription : class, IWebRequestReceiverDescription
    {
        public TDescription Description { get; }

        public abstract event WebRequestReceiverHandler Handler;

        protected WebRequestReceiver(TDescription description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public T GetDescription<T>() where T : class, IDescription
        {
            return (T)GetDescription();
        }

        public IDescription GetDescription()
        {
            return Description;
        }
    }
}
