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
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();
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
                        _connections.Add("device", Context.ConnectionId);
                    }
                    else
                    {
                        _connections.Add("user", Context.ConnectionId);
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

                foreach (var connectionId in _connections.GetConnections("user"))
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

        public async Task Freq(int frequency, string device)
        {
            try
            {

                foreach (var connectionId in _connections.GetConnections("device"))
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


        //public async Task GetChat(int user_id_src, int user_id_dst)
        //{
        //    try
        //    {


        //        List<MESSAGES> msgs = _context.MESSAGES.Where(m => (m.user_src == user_id_src &&  m.user_dst ==user_id_dst) || (m.user_dst == user_id_src && m.user_src == user_id_dst)).OrderByDescending(u => u.timestamp).Take(30).ToList();

        //        foreach (var connectionId in _connections.GetConnections(user_id_src.ToString()))
        //        {
        //            await Clients.Client(connectionId).SendAsync("GetChatResponse", msgs);
        //        }

        //        //await Clients.All.SendAsync("broadcastMessage", userID, message);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.GetType()); // what is the real exception?
        //    }
        //    // Call the broadcastMessage method to update clients.

        //}

        //        public async Task GetUserId()
        //{
        //    try
        //    {
        //        string usr = Context.GetHttpContext().Request.Query["usercode"];
        //        USERS reg_user = _context.USERS.Where(m => m.email == usr).OrderByDescending(u => u.id).FirstOrDefault();
        //        //_connections.Add(reg_user.id.ToString(), Context.ConnectionId);

        //        foreach (var connectionId in _connections.GetConnections(reg_user.id.ToString()))
        //        {
        //            await Clients.Client(connectionId).SendAsync("GetUserIdResponse", reg_user.id.ToString());
        //        }

        //        //await Clients.All.SendAsync("broadcastMessage", userID, message);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.GetType()); // what is the real exception?
        //    }
        //    // Call the broadcastMessage method to update clients.

        //}

        //public async Task GetUsers(int user_id_src)
        //{
        //    try
        //    {
        //        List<USERS> reg_user = _context.USERS.Where(m => m.id != user_id_src).ToList();

        //        //_connections.Add(reg_user.id.ToString(), Context.ConnectionId);

        //        foreach (var connectionId in _connections.GetConnections(user_id_src.ToString()))
        //        {
        //            await Clients.Client(connectionId).SendAsync("GetUsersResponse", reg_user);
        //        }

        //        //await Clients.All.SendAsync("broadcastMessage", userID, message);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.GetType()); // what is the real exception?
        //    }
        //    // Call the broadcastMessage method to update clients.

        //}

        //public async Task GetChat(string userID)
        //{
        //    try
        //    {
        //        string usr = Context.GetHttpContext().Request.Query["usercode"];
        //        int dst = int.Parse(userID);
        //        int src = int.Parse(usr);

        //        List<MESSAGES> msgs = _context.MESSAGES.Where(m => (m.user_src == src && m.user_dst == dst) || (m.user_dst == src && m.user_src == dst)).OrderByDescending(u => u.timestamp).Take(30).ToList();

        //        foreach (var connectionId in _connections.GetConnections(usr))
        //        {
        //            await Clients.Client(connectionId).SendAsync("GetChatResponse", userID, msgs);
        //        }

        //        //await Clients.All.SendAsync("broadcastMessage", userID, message);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.GetType()); // what is the real exception?
        //    }
        //    // Call the broadcastMessage method to update clients.

        //}
    }
}