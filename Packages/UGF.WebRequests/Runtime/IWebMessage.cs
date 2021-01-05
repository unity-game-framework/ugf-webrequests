using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public interface IWebMessage
    {
        IReadOnlyDictionary<string, string> Headers { get; }
        object Data { get; }
        bool HasData { get; }
    }
}
