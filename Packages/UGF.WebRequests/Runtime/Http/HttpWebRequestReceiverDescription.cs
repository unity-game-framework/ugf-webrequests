using System.Collections.Generic;
using UGF.Description.Runtime;

namespace UGF.WebRequests.Runtime.Http
{
    public class HttpWebRequestReceiverDescription : DescriptionBase, IWebRequestReceiverDescription
    {
        public List<string> Prefixes { get; } = new List<string>();
    }
}
