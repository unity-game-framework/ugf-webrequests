using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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

        protected override async Task<IWebResponse> OnSendAsync(IWebRequest request)
        {
            UnityWebRequest unityWebRequest = OnCreateWebRequest(request);

            m_currentUnityWebRequest = unityWebRequest ?? throw new ArgumentNullException(nameof(unityWebRequest), "Value cannot be null or empty.");

            using (unityWebRequest)
            {
                unityWebRequest.SendWebRequest();

                while (!unityWebRequest.isDone)
                {
                    await Task.Yield();
                }

                IWebResponse response = OnCreateResponse(request, unityWebRequest);

                if (response == null) throw new ArgumentNullException(nameof(response), "Value cannot be null or empty.");

                m_currentUnityWebRequest = null;

                return response;
            }
        }

        protected virtual UnityWebRequest OnCreateWebRequest(IWebRequest request)
        {
            string method = WebRequestUtility.GetMethodName(request.Method);
            var unityWebRequest = new UnityWebRequest(request.Url, method);

            foreach (KeyValuePair<string, string> pair in request.Headers)
            {
                unityWebRequest.SetRequestHeader(pair.Key, pair.Value);
            }

            OnCreateUploadHandler(request, unityWebRequest);
            OnCreateDownloadHandler(request, unityWebRequest);

            return unityWebRequest;
        }

        protected virtual void OnCreateUploadHandler(IWebRequest request, UnityWebRequest unityWebRequest)
        {
        }

        protected virtual void OnCreateDownloadHandler(IWebRequest request, UnityWebRequest unityWebRequest)
        {
        }

        protected virtual IWebResponse OnCreateResponse(IWebRequest request, UnityWebRequest unityWebRequest)
        {
            Dictionary<string, string> headers = unityWebRequest.GetResponseHeaders();
            var statusCode = (HttpStatusCode)unityWebRequest.responseCode;
            var response = new WebResponse(headers, request.Method, request.Url, statusCode);

            return response;
        }
    }
}
