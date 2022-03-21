using System;
using UGF.Description.Runtime;

namespace UGF.WebRequests.Runtime.Http
{
    public class HttpWebRequestSenderDescription : DescriptionBase, IWebRequestSenderDescription
    {
        public bool OverrideTimeout { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100D);
        public bool OverrideMaxResponseContentBufferSize { get; set; }
        public int MaxResponseContentBufferSize { get; set; } = int.MaxValue;
    }
}
