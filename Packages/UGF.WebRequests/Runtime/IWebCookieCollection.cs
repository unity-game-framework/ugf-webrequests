using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public interface IWebCookieCollection<TCookie> : IReadOnlyList<TCookie> where TCookie : IWebCookie
    {
        void Add(TCookie cookie);
        bool Remove(string name);
        void Set(string name, TCookie cookie);
        TCookie Get(string name);
        bool TryGet(string name, out TCookie cookie);
        bool TryGet(string name, out TCookie cookie, out int index);
        TCookie GetByValue(string value);
        bool TryGetByValue(string value, out TCookie cookie);
        bool TryGetByValue(string value, out TCookie cookie, out int index);
        string Format();
    }
}
