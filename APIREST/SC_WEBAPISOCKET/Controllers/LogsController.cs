using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using SC_WEBAPISOCKET.DAL;
using SC_WEBAPISOCKET.Models;
using System.Collections.Specialized;
using System.Web;
using MongoDB.Bson;
using Amazon.Auth.AccessControlPolicy;
using Microsoft.Azure.Cosmos;
using SC_WEBAPISOCKET.Helpers;
using System.Text.Json;
using static MongoDB.Driver.WriteConcern;
using Newtonsoft.Json.Linq;

namespace SC_WEBAPISOCKET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogsController : ControllerBase
    {


        [HttpPost("Get")]
        public async Task<IActionResult> GetHist(USER userRequest)
        {
            try
            {
                AuthController auth = new AuthController();
                OkObjectResult res = (OkObjectResult)await auth.ValidateUser(userRequest);
                if (res.StatusCode == 200)
                {
                    var client = new MongoClient(Config.ConnectString);

                    var collection = client.GetDatabase("test").GetCollection<Reg>("registers");

                    var listt = collection.Find<Reg>(_ => true).ToList<Reg>();

                    return Ok(listt);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("Devices")]
        public async Task<IActionResult> GetDevices(USER userRequest)
        {
            try
            {
                AuthController auth = new AuthController();
                OkObjectResult res = (OkObjectResult)await auth.ValidateUser(userRequest);
                if (res.StatusCode == 200)
                {
                    var client = new MongoClient(Config.ConnectString);

                    var collection = client.GetDatabase("test").GetCollection<Reg>("registers");

                    LogsReg logsReg = new LogsReg();

                    var filter = Builders<Reg>.Filter.Eq(r => r.deviceId, "1") & Builders<Reg>.Filter.Gt(r => r.timestamp, DateTime.Now.AddMinutes(-10));
                    logsReg.Device1 = collection.Find<Reg>(filter).SortBy(i => i.timestamp).ToList<Reg>();
                    var filter2 = Builders<Reg>.Filter.Eq(r => r.deviceId, "2") & Builders<Reg>.Filter.Gt(r => r.timestamp, DateTime.Now.AddMinutes(-10));
                    logsReg.Device2 = collection.Find<Reg>(filter).SortBy(i => i.timestamp).ToList<Reg>();
                    var filter3 = Builders<Reg>.Filter.Eq(r => r.deviceId, "3") & Builders<Reg>.Filter.Gt(r => r.timestamp, DateTime.Now.AddMinutes(-10));
                    logsReg.Device3 = collection.Find<Reg>(filter).SortBy(i=> i.timestamp).ToList<Reg>();
                    var filter4 = Builders<Reg>.Filter.Eq(r => r.deviceId, "4") & Builders<Reg>.Filter.Gt(r => r.timestamp, DateTime.Now.AddMinutes(-10));
                    logsReg.Device4 = collection.Find<Reg>(filter).SortBy(i => i.timestamp).ToList<Reg>();

                    return Ok(logsReg);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> DeleteHist(USER userRequest)
        {
            try
            {
                AuthController auth = new AuthController();
                OkObjectResult res = (OkObjectResult)await auth.ValidateUser(userRequest);
                if (res.StatusCode == 200)
                {
                    var client = new MongoClient(Config.ConnectString);

                    var collection = client.GetDatabase("test").GetCollection<Reg>("registers");

                    collection.DeleteMany(_ => true);

                    return Ok();
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }






    }
}
