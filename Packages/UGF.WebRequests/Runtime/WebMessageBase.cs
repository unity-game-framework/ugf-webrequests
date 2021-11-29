using System;
using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public abstract class WebMessageBase : IWebMessage
    {
        public Dictionary<string, string> Headers { get; }
        public object Data { get { return m_data ?? throw new ArgumentException("Value not specified."); } }
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

        public T GetData<T>()
        {
            return (T)GetData();
        }

        public object GetData()
        {
            return TryGetData(out object data) ? data : throw new ArgumentException("Data not specified.");
        }

        public bool TryGetData<T>(out T data)
        {
            if (TryGetData(out object value))
            {
                data = (T)value;
                return true;
            }

            data = default;
            return false;
        }

        public bool TryGetData(out object data)
        {
            if (m_data != null)
            {
                data = m_data;
                return true;
            }

            data = default;
            return false;
        }
    }
}
