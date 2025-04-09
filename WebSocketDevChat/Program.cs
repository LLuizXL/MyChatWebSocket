using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using WebSocketDevChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowCredentials().WithOrigins("http://localhost:5173", "http://192.168.0.121:5173/");
        });
});

builder.Services.AddSignalR();

var app = builder.Build();


app.UseCors();

app.MapHub<ChatHub>("/chat");




app.Run();
