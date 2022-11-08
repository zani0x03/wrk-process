namespace wrk_process.Models;


public class ProductOrder{
    public Guid ProductId { get; set; }
    public int Amount { get; set; }
    public Double TotalValue { get; set; }

}