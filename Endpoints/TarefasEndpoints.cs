using TarefasApi.Data;
using static TarefasApi.Data.TarefaContext;
using Dapper.Contrib.Extensions;

namespace TarefasApi.Endpoints;

public static class TarefasEndpoints
{
    public static void MapTerefasEndPoints(this WebApplication app)
    {
        app.MapGet("/tarefas", async (GetConnection connetionGetter) => {
            using var con = await connetionGetter();
            return con.GetAll<Tarefa>().ToList();
        });

        app.MapGet("/tarefas/{id}", async (GetConnection connectionGetter, int id) => {
            using var con = await connectionGetter();
            return con.Get<Tarefa>(id);
        });

        app.MapPost("/tarefas", async (GetConnection connectionGetter, Tarefa tarefa) => {
            using var con = await connectionGetter();
            var id = con.Insert(tarefa);

            return Results.Created($"/tarefas/{id}", tarefa);
        });

        app.MapPut("/tarefas", async (GetConnection connectionGetter, Tarefa tarefa) => {
            using var con = await connectionGetter();
            var id = con.Update(tarefa);

            return Results.Ok("Atualizado com sucesso");
        });

        app.MapDelete("/tarefas", async (GetConnection connectionGetter, int id) => {
            using var con = await connectionGetter();
            con.Delete(new Tarefa(id, "",""));

            return Results.Ok("Tarefa excluída com sucesso");
        });
    }
}