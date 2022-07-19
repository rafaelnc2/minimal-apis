//var builder = WebApplication.CreateBuilder(args);
using CatalogoApi;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureWebHostDefaults(WebApplicationBuilder => {
    WebApplicationBuilder.UseStartup<Startup>();
});

var app = builder.Build();

app.Run();