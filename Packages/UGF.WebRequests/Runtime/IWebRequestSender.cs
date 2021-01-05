using System.Threading.Tasks;
using UGF.Description.Runtime;
using UGF.Initialize.Runtime;

namespace UGF.WebRequests.Runtime
{
    public interface IWebRequestSender : IInitialize, IDescribed
    {
        Task<IWebResponse> SendAsync(IWebRequest request);
    }
}
