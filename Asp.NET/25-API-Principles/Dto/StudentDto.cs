using System.ComponentModel.DataAnnotations;
namespace _25_API_Principles.Dto;

public class CreateStudentDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(20, MinimumLength = 3)]
    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name must contain letters only")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Department is required")]
    [StringLength(10,MinimumLength = 2)]
    [RegularExpression("^[A-Z]+$",ErrorMessage = "Department must be uppercase letters only")]
    public string Department { get; set; } = "";
}