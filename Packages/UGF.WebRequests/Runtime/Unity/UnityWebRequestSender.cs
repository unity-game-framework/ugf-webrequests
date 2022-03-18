using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UGF.Logs.Runtime;
using UGF.RuntimeTools.Runtime.Encodings;
using UnityEngine.Networking;

namespace UGF.WebRequests.Runtime.Unity
{
    public class UnityWebRequestSender : WebRequestSender<UnityWebRequestSenderDescription>
    {
        public UnityWebRequest CurrentUnityWebRequest { get { return m_currentUnityWebRequest ?? throw new InvalidOperationException("Has no Unity Web Request."); } }
        public bool HasCurrentUnityWebRequest { get { return m_currentUnityWebRequest != null; } }

        private UnityWebRequest m_currentUnityWebRequest;

        public UnityWebRequestSender(UnityWebRequestSenderDescription description) : base(description)
        {
        }

        protected override void OnUninitialize()
        {
            base.OnUninitialize();

            if (HasCurrentUnityWebRequest)
            {
                CurrentUnityWebRequest.Dispose();
            }
        }

        protected override async Task<IWebResponse> OnSendAsync(IWebRequest request)
        {
            using (UnityWebRequest unityWebRequest = OnCreateWebRequest(request))
            {
                m_currentUnityWebRequest = unityWebRequest ?? throw new ArgumentNullException(nameof(unityWebRequest), "Value cannot be null or empty.");

                Log.Debug("Sending web request", new
                {
                    request.Method,
                    request.Url,
                    request.HasData
                });

                unityWebRequest.SendWebRequest();

                while (!unityWebRequest.isDone)
                {
                    await Task.Yield();
                }

                string error = unityWebRequest.error;

                if (error != null)
                {
                    Log.Warning($"Unity Web Request error has occurred: '{error}'.");
                }

                IWebResponse response = await OnCreateResponseAsync(request, unityWebRequest);

                if (response == null) throw new ArgumentNullException(nameof(response), "Value cannot be null or empty.");

                m_currentUnityWebRequest = null;

                Log.Debug("Received web response", new
                {
                    response.Method,
                    response.Url,
                    response.StatusCode,
                    response.HasData
                });

                return response;
            }
        }

        protected virtual Task<IWebResponse> OnCreateResponseAsync(IWebRequest request, UnityWebRequest unityWebRequest)
        {
            Dictionary<string, string> headers = unityWebRequest.GetResponseHeaders() ?? new Dictionary<string, string>();
            var statusCode = (HttpStatusCode)unityWebRequest.responseCode;
            var response = new WebResponse(headers, request.Method, request.Url, statusCode);
            byte[] data = unityWebRequest.downloadHandler?.data;

            if (data == null)
            {
                string error = unityWebRequest.error;

                if (!string.IsNullOrEmpty(error))
                {
                    Encoding encoding = EncodingUtility.GetEncoding(Description.ErrorEncoding);

                    data = encoding.GetBytes(error);
                }
            }

            if (data != null && data.Length > 0)
            {
                response.SetData(data);
            }

            return Task.FromResult<IWebResponse>(response);
        }

        protected virtual UnityWebRequest OnCreateWebRequest(IWebRequest request)
        {
            string method = WebRequestUtility.GetMethodName(request.Method);

            var unityWebRequest = new UnityWebRequest(request.Url, method)
            {
                redirectLimit = Description.RedirectLimit,
                timeout = Description.Timeout,
                useHttpContinue = Description.UseHttpContinue
            };

            foreach ((string key, string value) in request.Headers)
            {
                unityWebRequest.SetRequestHeader(key, value);
            }

            OnCreateUploadHandler(request, unityWebRequest);
            OnCreateDownloadHandler(request, unityWebRequest);

            return unityWebRequest;
        }

        protected virtual void OnCreateUploadHandler(IWebRequest request, UnityWebRequest unityWebRequest)
        {
            if (request.TryGetData(out byte[] bytes))
            {
                var handler = new UploadHandlerRaw(bytes);

                if (request.Headers.TryGetValue(WebRequestHeaders.ContentType, out string value))
                {
                    handler.contentType = value;
                }

                unityWebRequest.uploadHandler = handler;
            }
        }

        protected virtual void OnCreateDownloadHandler(IWebRequest request, UnityWebRequest unityWebRequest)
        {
            if (request.Method == WebRequestMethod.Get
                || request.Method == WebRequestMethod.Post
                || request.Method == WebRequestMethod.Put)
            {
                unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            }
        }
    }
}
