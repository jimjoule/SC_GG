using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SC_WEBAPISOCKET.Controllers;
using SC_WEBAPISOCKET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using SC_WEBAPISOCKET.Helpers;

namespace SC_WEBAPISOCKET.WebSocket

{
    public class WSHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            try
            {
                string token = Context.GetHttpContext().Request.Query["token"];
                string? device = Context.GetHttpContext().Request.Query["device"];
                //USERS reg_user = _context.USERS.Where(m => m.email == usr).OrderByDescending(u => u.id).FirstOrDefault();
                AuthController auth = new AuthController();
                USER usr = new USER() { Token = token };
                OkObjectResult res = (OkObjectResult)await auth.ValidateUser(usr);
                if (res.StatusCode == 200 )
                {
                    if(device != null)
                    {
                        HubConnections.ConnectionsDevices.Add(device, Context.ConnectionId);
                    }
                    else
                    {
                        HubConnections.Connections.Add("user", Context.ConnectionId);
                    }
                    

                    base.OnConnectedAsync();
                }

                
            }
            catch(Exception ex)
            {
                this.Context.Abort();
            }

        }
        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    string name = Context.User.Identity.Name;

        //    _connections.Remove(name, Context.ConnectionId);

        //    return base.OnDisconnected(stopCalled);
        //}

        //public override Task OnReconnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(name, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}
        public async Task Send(string message)
        {
            try
            {
                var client = new MongoClient(Config.ConnectString);

                var collection = client.GetDatabase("test").GetCollection<BsonDocument>("registers");
                Reg r = JsonSerializer.Deserialize<Reg>(message);
                r.Id = Guid.NewGuid();
                collection.InsertOne(r.ToBsonDocument());

                foreach (var connectionId in HubConnections.Connections.GetConnections("user"))
                {
                    await Clients.Client(connectionId).SendAsync("SendResponse", message);
                }

                //foreach (var connectionId in _connections.GetConnections(user_id_src.ToString()))
                //{
                //    await Clients.Client(connectionId).SendAsync("SendResponse", message);
                //}

                //await Clients.All.SendAsync("broadcastMessage", message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType()); // what is the real exception?
            }
            // Call the broadcastMessage method to update clients.
           
        }

        public async Task HeartBeat()
        {
            try
            {
                foreach (var connectionId in HubConnections.Connections.GetConnections("user"))
                {

                   // var _strongChatHubContext = HttpContextAccessor.HttpContext.RequestServices.GetRequiredService<IHubContext<WSHub>>();

                    //await Clients.Client(connectionId).SendAsync("HeartBeat", _connectionsDevices.GetKeys());
                }

                //foreach (var connectionId in _connections.GetConnections(user_id_src.ToString()))
                //{
                //    await Clients.Client(connectionId).SendAsync("SendResponse", message);
                //}

                //await Clients.All.SendAsync("broadcastMessage", message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType()); // what is the real exception?
            }
            // Call the broadcastMessage method to update clients.

        }

        public async Task Freq(int frequency, string device)
        {
            try
            {

                

                foreach (var connectionId in HubConnections.ConnectionsDevices.GetConnections())
                {
                    await Clients.Client(connectionId).SendAsync("SendResponse", frequency, device);
                }

                //foreach (var connectionId in _connections.GetConnections(user_id_src.ToString()))
                //{
                //    await Clients.Client(connectionId).SendAsync("SendResponse", message);
                //}

                //await Clients.All.SendAsync("broadcastMessage", message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType()); // what is the real exception?
            }
            // Call the broadcastMessage method to update clients.

        }



    }
}