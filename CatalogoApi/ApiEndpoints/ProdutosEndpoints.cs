using CatalogoApi.Context;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.ApiEndpoints;

public static class ProdutosEndpoints
{
    public static void MapProdutosEndpoints(this WebApplication app)
    {
        app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
        {
            db.Produtos.Add(produto);
            await db.SaveChangesAsync();

            return Results.Created($"/produtos/{produto.ProdutoId}", produto);
        })
            .Produces<Produto>(StatusCodes.Status201Created)
            .WithName("Criar novo produto")
            .WithTags("Produtos");


        app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync())
            .Produces<List<Produto>>(StatusCodes.Status200OK)
            .WithName("Lista todos os produtos")
            .WithTags("Produtos");


        app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
            await db.Produtos.FindAsync(id) is Produto produto ? Results.Ok(produto) : Results.NotFound())
            .Produces<Produto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Retorna o produto por ID")
            .WithTags("Produtos");

        app.MapPut("/produtos/{id:int}/nome", async (int id, string produtoNome, AppDbContext db) =>
        {
            var produtoDb = await db.Produtos.SingleOrDefaultAsync(s => s.ProdutoId == id);

            if (produtoDb is null)
                return Results.NotFound("Produto não encontrado");

            produtoDb.Nome = produtoNome;

            await db.SaveChangesAsync();

            return Results.Ok(produtoDb);

        })
            .Produces<Produto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Atualiza nome produto")
            .WithTags("Produtos");

        app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
        {
            if (produto.ProdutoId != id)
                return Results.BadRequest();

            var produtoDb = await db.Produtos.FindAsync(id);

            if (produtoDb is null)
                return Results.NotFound();

            produtoDb.Nome = produto.Nome;
            produtoDb.Descricao = produto.Descricao;
            produtoDb.Preco = produto.Preco;
            produtoDb.Image = produto.Image;
            produtoDb.CategoriaId = produto.CategoriaId;

            await db.SaveChangesAsync();

            return Results.Ok(produtoDb);

        })
            .Produces<Produto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Atualiza dados produto")
            .WithTags("Produtos");

        app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
        {
            var produtoDb = await db.Produtos.FindAsync(id);

            if (produtoDb is null)
                return Results.NotFound();

            db.Produtos.Remove(produtoDb);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Exclui um produto")
            .WithTags("Produtos");


        app.MapGet("/produtosporpagina", async (int numPagina, int tamPagina, AppDbContext db) =>
            await db.Produtos
                .Skip((numPagina - 1) * tamPagina)
                .Take(tamPagina)
                .ToListAsync()
            )
            .WithName("Produtos por página")
            .WithTags("Produtos");
    }
}
