using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace UGF.WebRequests.Runtime
{
    public class WebRequestReceiveHandler : IWebRequestReceiveHandler
    {
        public Task<IWebResponse> ExecuteAsync(IWebRequestReceiver receiver, IWebRequest request)
        {
            if (receiver == null) throw new ArgumentNullException(nameof(receiver));
            if (request == null) throw new ArgumentNullException(nameof(request));

            return OnExecuteAsync(receiver, request);
        }

        protected virtual Task<IWebResponse> OnExecuteAsync(IWebRequestReceiver receiver, IWebRequest request)
        {
            var response = new WebResponse(request.Method, request.Url, HttpStatusCode.NotImplemented);

            foreach ((string key, string value) in request.Headers)
            {
                response.Headers.Add(key, value);
            }

            return Task.FromResult<IWebResponse>(response);
        }
    }
}
