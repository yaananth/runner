using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GitHub.Services.Common;
using GitHub.Services.WebApi;

namespace GitHub.DistributedTask.WebApi
{
    [ResourceArea(TaskResourceIds.AreaId)]
    public class RunServiceHttpClient : VssHttpClientBase
    {
        public RunServiceHttpClient(
            Uri baseUrl,
            VssCredentials credentials)
            : base(baseUrl, credentials)
        {
        }

        public RunServiceHttpClient(
            Uri baseUrl,
            VssCredentials credentials,
            VssHttpRequestSettings settings)
            : base(baseUrl, credentials, settings)
        {
        }

        public RunServiceHttpClient(
            Uri baseUrl,
            VssCredentials credentials,
            params DelegatingHandler[] handlers)
            : base(baseUrl, credentials, handlers)
        {
        }

        public RunServiceHttpClient(
            Uri baseUrl,
            VssCredentials credentials,
            VssHttpRequestSettings settings,
            params DelegatingHandler[] handlers)
            : base(baseUrl, credentials, settings, handlers)
        {
        }

        public RunServiceHttpClient(
            Uri baseUrl,
            HttpMessageHandler pipeline,
            Boolean disposeHandler)
            : base(baseUrl, pipeline, disposeHandler)
        {
        }

        public Task<Pipelines.AgentJobRequestMessage> GetJobMessageAsync(
            Uri requestUri,
            string messageId,
            CancellationToken cancellationToken = default)
        {
            HttpMethod httpMethod = new HttpMethod("POST");
            var payload = new {
                StreamID = messageId
            };

            var payloadJson = JsonUtility.ToString(payload);
            var requestContent = new System.Net.Http.StringContent(payloadJson, System.Text.Encoding.UTF8, "application/json");
            return SendAsync<Pipelines.AgentJobRequestMessage>(
                httpMethod,
                additionalHeaders: null,
                requestUri: requestUri,
                content: requestContent,
                cancellationToken: cancellationToken);
        }
    }
}
