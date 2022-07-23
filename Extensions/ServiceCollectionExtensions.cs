using System.Data.SqlClient;
using static TarefasApi.Data.TarefaContext;

namespace TarefasApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<GetConnection>(sp => 
        async () => {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var connections = new SqlConnection(connectionString);
            await connections.OpenAsync();

            return connections;
        });

        return builder;
    }
}