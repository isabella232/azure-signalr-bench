﻿using Microsoft.Azure.SignalR.Management;
using Plugin.Base;
using System.Threading.Tasks;

namespace Plugin.Microsoft.Azure.SignalR.Benchmark.AgentMethods
{
    public class RestBroadcast : RestBase, IAgentMethod
    {
        protected override Task<IServiceHubContext> CreateHubContextAsync()
        {
            return CreateHubContextHelperAsync(ServiceTransportType.Transient);
        }

        protected override async Task RestSendMessage(IServiceHubContext hubContext)
        {
            await RestBroadcastMessage(hubContext);
        }
    }
}
