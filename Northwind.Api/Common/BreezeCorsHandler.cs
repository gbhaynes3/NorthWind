using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Northwind.Api.Common
{
    public class BreezeCorsHandler : DelegatingHandler
    {
        const string Origin = "Origin";
        const string AccessControlRequestMethod = "Access-Control-Request-Method";
        const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";
        const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var isCorsRequest = request.Headers.Contains(Origin);
            var isPreflightRequest = request.Method == HttpMethod.Options;
            if (isCorsRequest)
            {
                if (isPreflightRequest)
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Headers.Add(AccessControlAllowOrigin,
                      request.Headers.GetValues(Origin).First());

                    var accessControlRequestMethod =
                      request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();

                    if (accessControlRequestMethod != null)
                    {
                        response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
                    }

                    var requestedHeaders = string.Join(", ",
                       request.Headers.GetValues(AccessControlRequestHeaders));

                    if (!string.IsNullOrEmpty(requestedHeaders))
                    {
                        response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
                    }

                    response.Headers.Add(AccessControlAllowCredentials, "true");

                    var tcs = new TaskCompletionSource<HttpResponseMessage>();
                    tcs.SetResult(response);
                    return tcs.Task;
                }
                return base.SendAsync(request, cancellationToken).ContinueWith(t =>
                {
                    var resp = t.Result;
                    resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                    resp.Headers.Add(AccessControlAllowCredentials, "true");
                    return resp;
                });
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}