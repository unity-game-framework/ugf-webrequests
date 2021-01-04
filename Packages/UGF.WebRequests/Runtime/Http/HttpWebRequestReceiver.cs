using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UGF.Initialize.Runtime;

namespace UGF.WebRequests.Runtime.Http
{
    public class HttpWebRequestReceiver : WebRequestReceiver<HttpWebRequestReceiverDescription>
    {
        public HttpListener Listener { get { return m_listener ?? throw new InitializeStateException("Listener is not initialized."); } }

        private HttpListener m_listener;

        public HttpWebRequestReceiver(HttpWebRequestReceiverDescription description, IWebRequestReceiveHandler handler) : base(description, handler)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            m_listener = OnCreateListener();
            m_listener.Start();

            StartListen();
        }

        protected override void OnUninitialize()
        {
            base.OnUninitialize();

            m_listener = null;
        }

        protected virtual HttpListener OnCreateListener()
        {
            var listener = new HttpListener();

            foreach (string prefix in Description.Prefixes)
            {
                listener.Prefixes.Add(prefix);
            }

            return listener;
        }

        protected virtual async Task OnListenAsync()
        {
            HttpListener listener = Listener;

            while (listener.IsListening)
            {
                HttpListenerContext context = await listener.GetContextAsync();

                if (m_listener != null)
                {
                    try
                    {
                        IWebRequest request = await OnCreateRequestAsync(context);
                        IWebResponse response = await OnCreateResponseAsync(request, context);

                        await OnProcessResponse(response, context);
                    }
                    catch (Exception exception)
                    {
                        Console.Write(exception);
                    }
                }
                else
                {
                    listener.Close();
                }
            }
        }

        protected virtual async Task<IWebRequest> OnCreateRequestAsync(HttpListenerContext context)
        {
            HttpListenerRequest listenerRequest = context.Request;
            WebRequestMethod method = WebRequestUtility.GetMethod(listenerRequest.HttpMethod);
            var request = new WebRequest(method, listenerRequest.RawUrl);

            foreach (string key in listenerRequest.Headers.AllKeys)
            {
                string value = listenerRequest.Headers.Get(key);

                request.Headers.Add(key, value);
            }

            if (listenerRequest.HasEntityBody)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await listenerRequest.InputStream.CopyToAsync(memoryStream);

                    byte[] bytes = memoryStream.ToArray();

                    request.SetData(bytes);
                }
            }

            return request;
        }

        protected virtual Task<IWebResponse> OnCreateResponseAsync(IWebRequest request, HttpListenerContext context)
        {
            return HandleRequestAsync(request);
        }

        protected virtual Task OnProcessResponse(IWebResponse response, HttpListenerContext context)
        {
            HttpListenerResponse listenerResponse = context.Response;

            listenerResponse.StatusCode = (int)response.StatusCode;

            foreach (KeyValuePair<string, string> pair in response.Headers)
            {
                listenerResponse.Headers[pair.Key] = pair.Value;
            }

            if (response.HasData)
            {
                if (response.Data is byte[] bytes)
                {
                    listenerResponse.ContentLength64 = bytes.Length;

                    using (var memoryStream = new MemoryStream(bytes))
                    {
                        return memoryStream.CopyToAsync(listenerResponse.OutputStream);
                    }
                }

                throw new ArgumentException("Data must be a byte array.");
            }

            return Task.CompletedTask;
        }

        private async void StartListen()
        {
            await OnListenAsync();
        }
    }
}
