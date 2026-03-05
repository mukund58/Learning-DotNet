using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace _20_curd_mastery.Models;

public class Todo
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
