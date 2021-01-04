using UGF.Description.Runtime;
using UGF.Initialize.Runtime;

namespace UGF.WebRequests.Runtime
{
    public interface IWebRequestReceiver : IInitialize, IDescribed
    {
        IWebRequestReceiveHandler Handler { get; }
    }
}
