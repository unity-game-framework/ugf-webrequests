using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UGF.Initialize.Runtime;

namespace UGF.WebRequests.Runtime.Http
{
    public class HttpWebRequestSender : WebRequestSender<HttpWebRequestSenderDescription>
    {
        public HttpClient Client { get { return m_client ?? throw new InitializeStateException("Client is not initialized."); } }

        private HttpClient m_client;

        public HttpWebRequestSender(HttpWebRequestSenderDescription description) : base(description)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            m_client = OnCreateClient();
        }

        protected override void OnUninitialize()
        {
            base.OnUninitialize();

            m_client.Dispose();
            m_client = null;
        }

        protected override async Task<IWebResponse> OnSendAsync(IWebRequest request)
        {
            HttpClient client = Client;
            HttpRequestMessage requestMessage = OnCreateRequestMessage(request);

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

        protected virtual HttpClient OnCreateClient()
        {
            var client = new HttpClient
            {
                Timeout = Description.Timeout,
                MaxResponseContentBufferSize = Description.MaxResponseContentBufferSize
            };

            return client;
        }

        protected virtual HttpRequestMessage OnCreateRequestMessage(IWebRequest request)
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
