
using device;
using device.Models;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

//Get ConnString from AppSettings
IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
Config.WebSocket = config.GetConnectionString("WebSocket");
Config.User = config.GetConnectionString("User");
Config.Password = config.GetConnectionString("Password");

//Login Required for Token Auth
USER usr = new USER() { User = Config.User, Password = Config.Password };
usr = Login(usr).Result;

if(usr != null)
{
    //Initialize 4 devices
    Device d1 = new Device("1", usr.Token);
    Device d2 = new Device("2", usr.Token);
    Device d3 = new Device("3", usr.Token);
    Device mainApp = new Device("4", usr.Token);

    //Run Devices in diferent threads
    Thread thread = new Thread(new ThreadStart(() => {

        d1.Start().Wait();
    }));
    thread.Start();

    Thread thread2 = new Thread(new ThreadStart(() => {

        d2.Start().Wait();
    }));
    thread2.Start();

    Thread thread3 = new Thread(new ThreadStart(() => {
        
        d3.Start().Wait();
    }));
    thread3.Start();


    //Block Main Thread
    mainApp.Start().Wait();
}

//Login Call for API Authentfication
async Task<USER> Login(USER usr)
{
    try
    {
        //Set Serializer to Cas Insensitive
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        //Serialize USER object
        string jsonString = JsonSerializer.Serialize(usr);

        //Create HTTP Client
        HttpClient myHttpClient = new HttpClient();
        HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        //Send POST Call
        HttpResponseMessage response2 = await myHttpClient.PostAsync(Config.WebSocket+"/auth/login", content);
        
        //Check response API 200
        if (!response2.IsSuccessStatusCode) return null;

        //Return USER Obejct with token
        string resp = await response2.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<USER>(resp, options);
    }
    catch (Exception ex)
    {
        return null;
    }
}