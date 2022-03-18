using UGF.Description.Runtime;
using UGF.RuntimeTools.Runtime.Encodings;

namespace UGF.WebRequests.Runtime.Unity
{
    public class UnityWebRequestSenderDescription : DescriptionBase, IWebRequestSenderDescription
    {
        public int RedirectLimit { get; set; } = 32;
        public int Timeout { get; set; }
        public bool UseHttpContinue { get; set; } = true;
        public EncodingType ErrorEncoding { get; set; } = EncodingType.Default;
    }
}
