using System;
using System.Collections;
using System.Collections.Generic;

namespace UGF.WebRequests.Runtime
{
    public abstract class WebCookieCollection<TCookie> : IWebCookieCollection<TCookie> where TCookie : IWebCookie
    {
        public int Count { get { return m_cookies.Count; } }

        public TCookie this[int index]
        {
            get { return m_cookies[index]; }
            set
            {
                if (!value.IsValid()) throw new ArgumentException("Value should be valid.", nameof(value));

                m_cookies[index] = value;
            }
        }

        private readonly List<TCookie> m_cookies;

        protected WebCookieCollection()
        {
            m_cookies = new List<TCookie>();
        }

        protected WebCookieCollection(int capacity)
        {
            m_cookies = new List<TCookie>(capacity);
        }

        protected WebCookieCollection(IEnumerable<TCookie> cookies)
        {
            m_cookies = new List<TCookie>(cookies);
        }

        public void Add(TCookie cookie)
        {
            m_cookies.Add(cookie);
        }

        public bool Remove(string name)
        {
            for (int i = 0; i < m_cookies.Count; i++)
            {
                TCookie cookie = m_cookies[i];

                if (cookie.Name == name)
                {
                    m_cookies.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void Set(string name, TCookie cookie)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (!cookie.IsValid()) throw new ArgumentException("Value should be valid.", nameof(cookie));

            if (TryGet(name, out _, out int index))
            {
                m_cookies[index] = cookie;
            }
            else
            {
                Add(cookie);
            }
        }

        public TCookie Get(string name)
        {
            return TryGet(name, out TCookie cookie) ? cookie : throw new ArgumentException($"Cookie not found by the specified name: '{name}'.");
        }

        public bool TryGet(string name, out TCookie cookie)
        {
            return TryGet(name, out cookie, out _);
        }

        public bool TryGet(string name, out TCookie cookie, out int index)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            for (int i = 0; i < m_cookies.Count; i++)
            {
                cookie = m_cookies[i];

                if (cookie.Name == name)
                {
                    index = i;
                    return true;
                }
            }

            cookie = default;
            index = default;
            return false;
        }

        public TCookie GetByValue(string value)
        {
            return TryGetByValue(value, out TCookie cookie) ? cookie : throw new ArgumentException($"Cookie not found by the specified value: '{value}'.");
        }

        public bool TryGetByValue(string value, out TCookie cookie)
        {
            return TryGetByValue(value, out cookie, out _);
        }

        public bool TryGetByValue(string value, out TCookie cookie, out int index)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            for (int i = 0; i < m_cookies.Count; i++)
            {
                cookie = m_cookies[i];

                if (cookie.Value == value)
                {
                    index = i;
                    return true;
                }
            }

            cookie = default;
            index = default;
            return false;
        }

        public string Format()
        {
            return OnFormat();
        }

        public List<TCookie>.Enumerator GetEnumerator()
        {
            return m_cookies.GetEnumerator();
        }

        protected abstract string OnFormat();

        IEnumerator<TCookie> IEnumerable<TCookie>.GetEnumerator()
        {
            return m_cookies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_cookies.GetEnumerator();
        }
    }
}
