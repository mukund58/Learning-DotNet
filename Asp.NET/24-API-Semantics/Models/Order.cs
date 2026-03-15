namespace _24_API_Semantics.Models;

public class Order
{
    public int Id { get; set; }
    public string ProductName { get; set; } = "";
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending";
}