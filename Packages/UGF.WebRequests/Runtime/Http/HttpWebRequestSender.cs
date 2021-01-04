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
            HttpRequestMessage requestMessage = OnCreateHttpRequestMessage(request);

            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage), "Value cannot be null or empty.");

            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
            IWebResponse response = await OnCreateResponseAsync(request, responseMessage);

            if (response == null) throw new ArgumentNullException(nameof(response), "Value cannot be null or empty.");

            return response;
        }

        protected virtual async Task<IWebResponse> OnCreateResponseAsync(IWebRequest request, HttpResponseMessage responseMessage)
        {
            var response = new WebResponse(request.Method, request.Url, responseMessage.StatusCode);

            foreach (KeyValuePair<string, IEnumerable<string>> pair in responseMessage.Headers)
            {
                string value = string.Join(",", pair.Value);

                response.Headers.Add(pair.Key, value);
            }

            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();

            response.SetData(bytes);

            return response;
        }

        protected virtual HttpClient OnCreateHttpClient()
        {
            var client = new HttpClient
            {
                Timeout = Description.Timeout,
                MaxResponseContentBufferSize = Description.MaxResponseContentBufferSize
            };

            return client;
        }

        protected virtual HttpRequestMessage OnCreateHttpRequestMessage(IWebRequest request)
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

            if (request.HasData)
            {
                if (request.Data is byte[] bytes)
                {
                    message.Content = new ByteArrayContent(bytes);
                }
                else
                {
                    throw new ArgumentException("Data must be a byte array.");
                }
            }

            return message;
        }
    }
}
