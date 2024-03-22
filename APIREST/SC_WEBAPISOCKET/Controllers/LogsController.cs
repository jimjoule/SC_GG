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
