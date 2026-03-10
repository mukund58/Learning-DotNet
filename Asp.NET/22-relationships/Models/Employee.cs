namespace _22_relationships.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // foreign key 
    public int DepartmentId { get; set; } 
    
    // Navigation Property
    // Using 'null!' tells the compiler EF will handle this, 
    // or use 'Department?' if the department is optional.
    public Department Department { get; set; } = null!;
}