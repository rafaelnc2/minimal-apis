using CatalogoApi.Models;
using CatalogoApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace CatalogoApi.ApiEndpoints;

public static class AutenticacaoEndpoints
{
    public static void MapAutenticacaoEndpoints(this WebApplication app)
    {
        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if (userModel is null)
                return Results.BadRequest("Login inválido");

            if (userModel.UserName.Equals("usuario") && userModel.Password.Equals("123456789"))
            {
                var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"], app.Configuration["Jwt:Issuer"], app.Configuration["Jwt:Audience"], userModel);

                return Results.Ok(new { token = tokenString });
            }

            return Results.BadRequest("Login inválido");
        })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("Login");
    }
}
