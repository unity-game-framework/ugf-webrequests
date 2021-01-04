using System.Threading.Tasks;

namespace UGF.WebRequests.Runtime
{
    public interface IWebRequestReceiveHandler
    {
        Task<IWebResponse> ExecuteAsync(IWebRequestReceiver receiver, IWebRequest request);
    }
}
