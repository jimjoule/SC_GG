using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SC_WEBAPISOCKET.Models;
using SC_WEBAPISOCKET.Helpers;

namespace SC_WEBAPISOCKET.DAL
{
    public class DBContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(
                Config.ConnectString,
                 "test");


        public DbSet<USER> USER { get; set; }



    }
}
