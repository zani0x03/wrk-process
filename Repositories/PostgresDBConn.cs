

using Npgsql;
using wrk_process.Repositories.Interfaces;

namespace wrk_process.Repositories;

public class PostgresDBConn:IPostgresDBConn
{

    private NpgsqlConnection _postgresConnection = null;



    public PostgresDBConn(){
        if (_postgresConnection == null){
            _postgresConnection = new NpgsqlConnection(Environment.GetEnvironmentVariable("POSTGRESSDB"));            
        }
    }

    public NpgsqlConnection RetPostgressConnection (){
        return _postgresConnection;
    }
}

