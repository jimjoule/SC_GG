using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SC_WEBAPISOCKET.Helpers;
using SC_WEBAPISOCKET.WebSocket;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.Swagger;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

builder.Services.AddControllers();

builder.Services.AddHostedService<WorkerService>();

builder.Services.AddAuthentication(cfg => {
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
            .GetBytes(builder.Configuration.GetValue<string>("Config:JWT_Secret"))
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.

Config.ConnectString = builder.Configuration.GetValue<string>("Config:ConnectString");
Config.PathLogs = builder.Configuration.GetValue<string>("Config:PathLogs");
Config.JWT_Secret = builder.Configuration.GetValue<string>("Config:JWT_Secret");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<WSHub>("chat");
});

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "";
});

app.Run();
