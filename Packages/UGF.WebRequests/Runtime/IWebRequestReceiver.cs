using UGF.Description.Runtime;
using UGF.Initialize.Runtime;

namespace UGF.WebRequests.Runtime
{
    public interface IWebRequestReceiver : IInitialize, IDescribed
    {
        event WebRequestReceiverHandler Handler;
    }
}
