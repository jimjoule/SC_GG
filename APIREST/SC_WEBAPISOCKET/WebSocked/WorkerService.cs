using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Connections;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SC_WEBAPISOCKET.WebSocket
{
    public class WorkerService : BackgroundService
    {
        private const int generalDelay = 1000; // 1 day in miliseconds

        private readonly IHubContext<WSHub> _hubContext;

        public WorkerService(IHubContext<WSHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(generalDelay, stoppingToken);
                await DoBackupAsync();
            }
        }

        private Task DoBackupAsync()
        {
            foreach (var connectionId in HubConnections.Connections.GetConnections("user"))
            {
                _hubContext.Clients.Client(connectionId).SendAsync("HeartBeat", HubConnections.ConnectionsDevices.GetKeys());
                // var _strongChatHubContext = HttpContextAccessor.HttpContext.RequestServices.GetRequiredService<IHubContext<WSHub>>();

                //await Clients.Client(connectionId).SendAsync("HeartBeat", _connectionsDevices.GetKeys());
            }

            return Task.FromResult("Done");
        }

    }
}
