using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UGF.Initialize.Runtime;
using UGF.Logs.Runtime;

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

            Log.Debug("Http client created", new
            {
                m_client.Timeout,
                m_client.BaseAddress,
                m_client.MaxResponseContentBufferSize
            });
        }

        protected override void OnUninitialize()
        {
            base.OnUninitialize();

            m_client.Dispose();
            m_client = null;

            Log.Debug("Http client disposed.");
        }

        protected override async Task<IWebResponse> OnSendAsync(IWebRequest request)
        {
            Log.Debug("Sending web request", new
            {
                request.Method,
                request.Url,
                request.HasData
            });

            HttpClient client = Client;
            HttpRequestMessage requestMessage = OnCreateRequestMessage(request);

            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage), "Value cannot be null or empty.");

            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
            IWebResponse response = await OnCreateResponseAsync(request, responseMessage);

            if (response == null) throw new ArgumentNullException(nameof(response), "Value cannot be null or empty.");

            Log.Debug("Received web response", new
            {
                response.Method,
                response.Url,
                response.StatusCode,
                response.HasData
            });

            return response;
        }

        protected virtual async Task<IWebResponse> OnCreateResponseAsync(IWebRequest request, HttpResponseMessage responseMessage)
        {
            var response = new WebResponse(request.Method, request.Url, responseMessage.StatusCode);

            foreach ((string key, IEnumerable<string> enumerable) in responseMessage.Headers)
            {
                string value = string.Join(",", enumerable);

                response.Headers.Add(key, value);
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

            foreach ((string key, string value) in request.Headers)
            {
                if (message.Headers.Contains(key))
                {
                    message.Headers.Remove(key);
                }

                message.Headers.Add(key, value);
            }

            if (request.TryGetData(out byte[] bytes))
            {
                message.Content = new ByteArrayContent(bytes);
            }

            return message;
        }
    }
}
