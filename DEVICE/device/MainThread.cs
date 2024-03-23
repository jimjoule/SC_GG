using device;
using device.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace device
{
    public class MainProgram
    {
        private string _deviceId;
        private WSClient _client = new WSClient();
        private Thread[] _childs;
        private Device[] _devices;

        public MainProgram(string id, string token, Thread[] childs, Device[] devices)
        {
            _deviceId = id;
            _childs = childs;
            _devices = devices;

            //Coonect to WebSocket
            _client._eventInterrupt += ThreadInterruption;
            _client.Connect_Main(token, _deviceId);
        }

        public async Task Start()
        {
            //Inifite Loop
            while (true)
            {
                //Delay to generate next data
                Thread.Sleep(_client.delay * 1000);
            }
        }

        private void ThreadInterruption(object? sender, EventArgs? e)
        {
            try
            {
                KeyValuePair<string, int> parameter = (KeyValuePair<string, int>)sender;
                int deviceid = int.Parse((string)parameter.Key);
                _devices[deviceid-1].SetInterval(parameter.Value);
                _childs[deviceid-1].Interrupt();
            }
            catch
            {

            }
        }
    }

   
}

