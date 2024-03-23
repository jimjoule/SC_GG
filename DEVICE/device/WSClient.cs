using device.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace device
{
    public class WSClient
    {
        public int delay { get; set; } = 5;
        private HubConnection _hubConnection { get; set; }

        private string _device { get; set; } 
        public EventHandler? _eventInterrupt;

        public void Connect(string token, string device)
        {
            try
            {
                //Setup WebSocket Connection
                _device = device;
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(Config.WebSocket+"/chat?token=" +token+"&device="+device)
                    .WithAutomaticReconnect()
                    .Build();

                //Start WebSocket Client Connection
                _hubConnection.StartAsync().Wait();

                ////Setup Reciving Messages From Server to Device
                //_hubConnection.On<int, string>("SendResponse", (Number, Device) =>
                //{
                //    if (Device == _device)
                //    {
                //        delay = Number;
                //    }

                //});

            
            }
            catch (Exception ex)
            {

            }
        }

        public void Connect_Main(string token, string device)
        {
            try
            {
                //Setup WebSocket Connection
                _device = device;
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(Config.WebSocket + "/chat?token=" + token + "&device=" + device)
                    .WithAutomaticReconnect()
                    .Build();

                //Start WebSocket Client Connection
                _hubConnection.StartAsync().Wait();


                //Setup Reciving Messages From Server to Device
                _hubConnection.On<int, string>("SendResponse", (Number, Device) =>
                {
                    KeyValuePair<string, int> parameter = new KeyValuePair<string, int>(Device, Number);
                    _eventInterrupt?.Invoke(parameter, null);
                });



            }
            catch (Exception ex)
            {

            }
        }

        public  async Task SendData(Register reg)
        {
            //Serialize Data
            string sReg = JsonSerializer.Serialize(reg);

            //Send Data to Server via WebSocket
            await _hubConnection.InvokeAsync("Send", sReg);
        }
    }
}
