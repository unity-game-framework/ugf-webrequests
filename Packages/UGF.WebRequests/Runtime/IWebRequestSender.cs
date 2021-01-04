using System.Threading.Tasks;
using UGF.Description.Runtime;

namespace UGF.WebRequests.Runtime
{
    public interface IWebRequestSender : IDescribed
    {
        Task<IWebResponse> SendAsync(IWebRequest request);
    }
}
