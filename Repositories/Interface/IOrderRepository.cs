using wrk_process.Models;

namespace wrk_process.Repositories.Interfaces;

public interface IOrderRepository{
    Task<Order> ReturnOrder(string orderId);
}