using System.Data;
using System.Threading.Tasks;

namespace TarefasApi.Data;

public class TarefaContext 
{
    public delegate Task<IDbConnection> GetConnection();
}