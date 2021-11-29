using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UGF.Initialize.Runtime;
using UGF.Logs.Runtime;

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

            Log.Debug("Http listener created and started", new
            {
                prefixesCount = m_listener.Prefixes.Count
            });
        }

        protected override void OnUninitialize()
        {
            base.OnUninitialize();

            m_listener.Stop();
            m_listener.Close();
            m_listener = null;

            Log.Debug("Http listener closed.");
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
                Log.Debug("Http listener processing.");

                try
                {
                    HttpListenerContext context = await listener.GetContextAsync();

                    try
                    {
                        IWebRequest request = await OnCreateRequestAsync(context);
                        IWebResponse response = await OnCreateResponseAsync(request, context);

                        await OnProcessResponse(response, context);
                    }
                    catch (Exception exception)
                    {
                        await OnProcessError(exception, context);
                    }
                }
                catch (Exception exception)
                {
                    Log.Warning("Listener processing error has occurred.", exception);

                    await Task.Yield();
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
                await using var memoryStream = new MemoryStream();

                await listenerRequest.InputStream.CopyToAsync(memoryStream);

                byte[] bytes = memoryStream.ToArray();

                request.SetData(bytes);
            }

            Log.Debug("Received web request", new
            {
                request.Method,
                request.Url,
                request.HasData
            });

            return request;
        }

        protected virtual async Task<IWebResponse> OnCreateResponseAsync(IWebRequest request, HttpListenerContext context)
        {
            IWebResponse response = await HandleRequestAsync(request);

            Log.Debug("Sending web response", new
            {
                response.Method,
                response.Url,
                response.StatusCode,
                response.HasData
            });

            return response;
        }

        protected virtual async Task OnProcessResponse(IWebResponse response, HttpListenerContext context)
        {
            HttpListenerResponse listenerResponse = context.Response;

            listenerResponse.StatusCode = (int)response.StatusCode;

            foreach ((string key, string value) in response.Headers)
            {
                listenerResponse.Headers[key] = value;
            }

            if (response.TryGetData(out byte[] bytes))
            {
                listenerResponse.ContentLength64 = bytes.Length;

                await using var memoryStream = new MemoryStream(bytes);

                await memoryStream.CopyToAsync(listenerResponse.OutputStream);
            }

            listenerResponse.OutputStream.Close();
        }

        protected virtual Task OnProcessError(Exception exception, HttpListenerContext context)
        {
            HttpListenerResponse listenerResponse = context.Response;

            listenerResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            listenerResponse.OutputStream.Close();

            Log.Warning("Listener request error has occurred", exception, new
            {
                context.Request.HttpMethod,
                context.Request.RawUrl,
                context.Request.HasEntityBody
            });

            return Task.CompletedTask;
        }

        private async void StartListen()
        {
            await OnListenAsync();
        }
    }
}
