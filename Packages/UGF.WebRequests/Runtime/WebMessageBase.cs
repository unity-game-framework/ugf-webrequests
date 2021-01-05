using System;
using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public abstract class WebMessageBase : IWebMessage
    {
        public Dictionary<string, string> Headers { get; }
        public object Data { get { return m_data ?? throw new InvalidOperationException("Data not specified."); } }
        public bool HasData { get { return m_data != null; } }

        IReadOnlyDictionary<string, string> IWebMessage.Headers { get { return Headers; } }

        private object m_data;

        protected WebMessageBase(Dictionary<string, string> headers)
        {
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }

        public void SetData(object data)
        {
            m_data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public void ClearData()
        {
            m_data = null;
        }
    }
}
