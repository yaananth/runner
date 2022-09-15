﻿using System;
using System.Threading;
using System.Threading.Tasks;
using GitHub.DistributedTask.Pipelines;
using GitHub.DistributedTask.WebApi;
using GitHub.Services.Common;
using GitHub.Services.WebApi;

namespace GitHub.Runner.Common
{
    [ServiceLocator(Default = typeof(RunServer))]
    public interface IRunServer : IRunnerService
    {
        Task ConnectAsync(Uri serverUrl, VssCredentials credentials);

        Task<AgentJobRequestMessage> GetJobMessageAsync(string id, CancellationToken token);
    }

    public sealed class RunServer : RunnerService, IRunServer
    {
        private bool _hasConnection;
        private Uri requestUri;
        private VssConnection _connection;
        private RunServiceHttpClient _runServiceHttpClient;

        public async Task ConnectAsync(Uri serverUri, VssCredentials credentials)
        {
            requestUri = serverUri;

            _connection = await EstablishVssConnection(new Uri(serverUri.Authority), credentials, TimeSpan.FromSeconds(100), rawConnection: true);
            _runServiceHttpClient = await _connection.GetRawClientAsync<RunServiceHttpClient>();
            _hasConnection = true;
        }

        private void CheckConnection()
        {
            if (!_hasConnection)
            {
                throw new InvalidOperationException($"SetConnection");
            }
        }

        public Task<AgentJobRequestMessage> GetJobMessageAsync(string id, CancellationToken cancellationToken)
        {
            CheckConnection();
            var jobMessage = RetryRequest<AgentJobRequestMessage>(
                async () => await _runServiceHttpClient.GetJobMessageAsync(requestUri, id, cancellationToken), cancellationToken);
            return jobMessage;
        }

    }
}
