using Dapper;
using Npgsql;
using work_process.Models;
using wrk_process.Repositories.Interfaces;

namespace wrk_process.Repositories;


public class ClientAPIAddress:IClientAPIRepository{

    private readonly IPostgresDBConn _postgresDBConn;

    public ClientAPIAddress(IPostgresDBConn postgresDBConn){
        _postgresDBConn = postgresDBConn;
    }


    public async Task<ClientAPI> retClientAPIAddress(Guid id){
        //string query = "select id, webhook from ClientAPIAddress where id = @";
        string query = "select id, urlbase, urlcomplement from \"clientapiaddress\" where id = :id";

        var postConn = _postgresDBConn.RetPostgressConnection();


        await postConn.OpenAsync();

        var clientAPI = (await postConn.QueryAsync<ClientAPI>(query, new {
            id = id.ToString()
        })).FirstOrDefault();

        await postConn.CloseAsync();


        return clientAPI;
    }


}