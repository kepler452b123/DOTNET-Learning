using Microsoft.Data.SqlClient;

namespace DOTNET_GAMESTORE.Services;

public class SqlPipeline{
    private string _connectionString = "";

    public SqlPipeline(string connectionString){
        _connectionString = connectionString;
    }

    public SqlConnection Create(){
        return new SqlConnection(_connectionString);
    }
}

