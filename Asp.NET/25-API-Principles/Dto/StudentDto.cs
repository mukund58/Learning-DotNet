using System.ComponentModel.DataAnnotations;
namespace _25_API_Principles.Dto;
// model validation using data annotation
public class CreateStudentDto
{
    // [Required(ErrorMessage = "Name is required")]
    // [StringLength(20, MinimumLength = 3)]
    // [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name must contain letters only")]
    private string _name= string.Empty;
    public string Name 
    { 
        get => _name; 
        set => _name = value?.Trim()?? string.Empty; // Auto-trim on assignment
    }
    // [Required(ErrorMessage = "Department is required")]
    // [StringLength(10,MinimumLength = 2)]
    // [RegularExpression("^[A-Z]+$",ErrorMessage = "Department must be uppercase letters only")]
    private string _department= string.Empty;
    public string Department 
    {
        get => _department; 
        // set => _department = value?.Trim(); 
        set => _department = value?.Trim().ToUpper() ?? string.Empty;
    }
}