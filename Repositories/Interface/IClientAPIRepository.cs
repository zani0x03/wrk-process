using work_process.Models;

namespace wrk_process.Repositories.Interfaces;

public interface IClientAPIRepository{
    Task<ClientAPI> retClientAPIAddress(Guid id);   
}