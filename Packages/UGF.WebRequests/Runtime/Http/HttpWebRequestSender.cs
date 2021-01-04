using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UGF.Initialize.Runtime;

namespace UGF.WebRequests.Runtime.Http
{
    public class HttpWebRequestSender : WebRequestSender<HttpWebRequestSenderDescription>
    {
        public HttpClient HttpClient { get { return m_httpClient ?? throw new InitializeStateException("HttpClient not initialized."); } }

        private HttpClient m_httpClient;

        public HttpWebRequestSender(HttpWebRequestSenderDescription description) : base(description)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            m_httpClient = OnCreateHttpClient();
        }

        protected override void OnUninitialize()
        {
            base.OnUninitialize();

            m_httpClient.Dispose();
            m_httpClient = null;
        }

        protected override async Task<IWebResponse> OnSendAsync(IWebRequest request)
        {
            HttpClient client = HttpClient;
            HttpRequestMessage requestMessage = OnCreateHttpRequestMessage(request, client);

            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage), "Value cannot be null or empty.");

            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
            IWebResponse response = OnCreateResponse(request, client, responseMessage);

            if (response == null) throw new ArgumentNullException(nameof(response), "Value cannot be null or empty.");

            return response;
        }

        protected virtual HttpClient OnCreateHttpClient()
        {
            var httpClient = new HttpClient();

            return httpClient;
        }

        protected virtual HttpRequestMessage OnCreateHttpRequestMessage(IWebRequest request, HttpClient client)
        {
            string methodName = WebRequestUtility.GetMethodName(request.Method);
            var method = new HttpMethod(methodName);
            var message = new HttpRequestMessage(method, request.Url);

            foreach (KeyValuePair<string, string> pair in request.Headers)
            {
                if (message.Headers.Contains(pair.Key))
                {
                    message.Headers.Remove(pair.Key);
                }

                message.Headers.Add(pair.Key, pair.Value);
            }

            return message;
        }

        protected virtual IWebResponse OnCreateResponse(IWebRequest request, HttpClient client, HttpResponseMessage responseMessage)
        {
            var response = new WebResponse(request.Method, request.Url, responseMessage.StatusCode);

            foreach (KeyValuePair<string, IEnumerable<string>> pair in responseMessage.Headers)
            {
                string value = string.Join(",", pair.Value);

                response.Headers.Add(pair.Key, value);
            }

            return response;
        }
    }
}
