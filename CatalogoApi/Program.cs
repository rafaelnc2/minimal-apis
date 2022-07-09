using CatalogoApi.Context;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configure services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, serverVersion));

var app = builder.Build();

//definindo endpoints

#region Categorias
app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
})
    .WithTags("Categorias");

app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync())
    .WithTags("Categorias");

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
    await db.Categorias.FindAsync(id) is Categoria categoria ? Results.Ok(categoria) : Results.NotFound())
    .WithTags("Categorias");

app.MapGet("/categoriaprodutos", async (AppDbContext db) =>
    await db.Categorias.Include(c => c.Produtos).ToListAsync())
    .WithTags("Categorias");

app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
{
    if (categoria.CategoriaId != id)
        return Results.BadRequest();

    var categoriaDb = await db.Categorias.FindAsync(id);

    if (categoriaDb is null)
        return Results.NotFound();

    categoriaDb.Nome = categoria.Nome;
    categoriaDb.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();

    return Results.Ok(categoriaDb);
})
    .WithTags("Categorias");

app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    var categoriaDb = await db.Categorias.FindAsync(id);

    if (categoriaDb is null)
        return Results.NotFound();

    db.Categorias.Remove(categoriaDb);

    await db.SaveChangesAsync();

    return Results.NoContent();
})
    .WithTags("Categorias");
#endregion


#region Produtos
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
#endregion


// Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();