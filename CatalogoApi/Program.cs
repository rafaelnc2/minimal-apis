using CatalogoApi.ApiEndpoints;
using CatalogoApi.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwagger();

builder.AddPersistence();

builder.Services.AddCors();

builder.AddAutenticationJwt();

var app = builder.Build();

app.MapAutenticacaoEndpoints();

app.MapCategoriasEndpoints();

app.MapProdutosEndpoints();

var env = app.Environment;

app.UseEceptionHandling(env)
    .UseSwaggerEndpoints()
    .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();