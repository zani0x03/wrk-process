using Npgsql;

namespace wrk_process.Repositories.Interfaces;

public interface IPostgresDBConn
{
    NpgsqlConnection RetPostgressConnection ();
}
