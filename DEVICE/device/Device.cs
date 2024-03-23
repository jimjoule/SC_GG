using device;
using device.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace device
{
    public class Device
    {
        private string _deviceId;
        private WSClient _client = new WSClient();

        public Device(string id, string token)
        {
            _deviceId = id;

            //Coonect to WebSocket
            _client.Connect(token, _deviceId);
        }

        public async Task Start()
        {
            //Inifite Loop
            while (true)
            {
                try
                {
                    //Delay to generate next data
                    Thread.Sleep(_client.delay * 1000);

                    //Generate Data for device
                    Register reg = Helpers.GenerateData();
                    reg.deviceId = _deviceId;

                    //Send data to WebSocket
                    _client.SendData(reg);

                    //Log Data into console APP
                    Console.Write(JsonSerializer.Serialize(reg) + "\n");

                }
                catch (ThreadInterruptedException)
                {
                }
            }
        }

        public void SetInterval(int interval)
        {
            _client.delay = interval;
        }
        
    }

   
}

