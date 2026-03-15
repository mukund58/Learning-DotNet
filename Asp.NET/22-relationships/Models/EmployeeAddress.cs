namespace _22_relationships.Models;
public class EmployeeAddress
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}