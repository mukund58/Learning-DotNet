namespace _24_API_Semantics.Models;

[Serializable] 
public class Todo{
    public int Id { get; set; }
    
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}