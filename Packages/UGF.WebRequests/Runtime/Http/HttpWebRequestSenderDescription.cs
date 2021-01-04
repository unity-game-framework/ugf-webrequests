using System;
using UGF.Description.Runtime;

namespace UGF.WebRequests.Runtime.Http
{
    public class HttpWebRequestSenderDescription : DescriptionBase, IWebRequestSenderDescription
    {
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100D);
        public long MaxResponseContentBufferSize { get; set; } = int.MaxValue;
    }
}
