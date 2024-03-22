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
    public class AuthController : ControllerBase
    {

        private readonly string _pepper = "Temporal Pepper";
        private readonly int _iteration = 3;


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateUser(USER userRequest)
        {
            try
            {
                OkObjectResult res = (OkObjectResult)await ValidateUser(userRequest);
                if (res.StatusCode == 200)
                {
                    USER usr = new USER
                    {
                        Id = Guid.NewGuid(),
                        User = userRequest.User,
                        PasswordSalt = PasswordHasher.GenerateSalt()
                    };


                    usr.Password = PasswordHasher.ComputeHash(userRequest.Password, usr.PasswordSalt, _pepper, _iteration);

                    var client = new MongoClient(Config.ConnectString);

                    var collection = client.GetDatabase("test").GetCollection<BsonDocument>("users");

                    collection.InsertOne(usr.ToBsonDocument());


                    return Ok();
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser(USER userRequest)
        {
            try
            {

                var client = new MongoClient(Config.ConnectString);

                var collection = client.GetDatabase("test").GetCollection<USER>("users");

                var filter = Builders<USER>.Filter.Eq(r => r.User, userRequest.User);

                USER usr = collection.Find(filter).FirstOrDefault();

                if(usr != null)
                {
                   if(usr.Password == PasswordHasher.ComputeHash(userRequest.Password, usr.PasswordSalt, _pepper, _iteration))
                    {
                        string token = TokensHelpers.GenerateJWTToken(userRequest.User);
                        var update = Builders<USER>.Update.Set(restaurant => restaurant.Token,token);

                        collection.UpdateOne(filter, update);
                        USER usrresp = new USER() { Token = token };
                        return Ok(usrresp);
                    }
                   
                }

                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("Validate")]
        public async Task<IActionResult> ValidateUser(USER userRequest)
        {
            try
            {
                if( userRequest.Token != null)
                {
                    var client = new MongoClient(Config.ConnectString);

                    var collection = client.GetDatabase("test").GetCollection<USER>("users");

                    var filter = Builders<USER>.Filter.Eq(r => r.Token, userRequest.Token);

                    USER usr = collection.Find(filter).FirstOrDefault();

                    if (usr != null)
                    {
                        return Ok(TokensHelpers.ValidateToken(userRequest.Token));

                    }

                    return BadRequest();
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }



        [HttpGet("Get")]
        public async Task<IActionResult> GetUsers(USER userRequest)
        {
            try
            {
                OkObjectResult res = (OkObjectResult)await ValidateUser(userRequest);
                if (res.StatusCode == 200)
                {

                    var client = new MongoClient(Config.ConnectString);

                    var collection = client.GetDatabase("test").GetCollection<USER>("users");

                    var listt = collection.Find<USER>(_ => true).ToList<USER>();

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
        public async Task<IActionResult> DeleteUsers(USER userRequest)
        {
            try
            {
                OkObjectResult res = (OkObjectResult)await ValidateUser(userRequest);
                if (res.StatusCode == 200)
                {

                    var client = new MongoClient(Config.ConnectString);

                    var collection = client.GetDatabase("test").GetCollection<BsonDocument>("users");

                    collection.DeleteMany(_ => true);

                    USER usr = new USER
                    {
                        Id = Guid.NewGuid(),
                        User = "gguma",
                        PasswordSalt = PasswordHasher.GenerateSalt()
                    };

                    usr.Password = PasswordHasher.ComputeHash("bigup", usr.PasswordSalt, _pepper, _iteration);

                    collection.InsertOne(usr.ToBsonDocument());

                    USER device = new USER
                    {
                        Id = Guid.NewGuid(),
                        User = "device",
                        PasswordSalt = PasswordHasher.GenerateSalt()
                    };

                    device.Password = PasswordHasher.ComputeHash("bigup", usr.PasswordSalt, _pepper, _iteration);

                    collection.InsertOne(device.ToBsonDocument());

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
