using System.ComponentModel.DataAnnotations;

// Models/Post.cs
public class Post
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int UserId { get; set; }
    
    // This forces the caller to initialize the property during object creation. 
    public required User User { get; set; }   // navigation property
}
