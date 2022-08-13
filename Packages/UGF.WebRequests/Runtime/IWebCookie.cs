namespace UGF.WebRequests.Runtime
{
    public interface IWebCookie
    {
        string Name { get; }
        string Value { get; }

        bool IsValid();
    }
}
