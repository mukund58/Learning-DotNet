using System.ComponentModel.DataAnnotations;

// Models/User.cs
public class User
{
    public int UserId { get; set; }

    public required string? Name { get; set; }

}
