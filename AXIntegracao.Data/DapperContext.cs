using System.Data.SqlClient;

namespace AXIntegracao.Data
{
    public class DapperContext
    {
        public SqlConnection connection;
        public DapperContext(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }
    }
}
