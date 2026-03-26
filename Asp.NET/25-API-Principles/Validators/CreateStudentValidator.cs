namespace _25_API_Principles.Validators;
using FluentValidation;
using _25_API_Principles.Dto;


public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Name)
            // .Transform(x => x?.Trim()) // 1. Clean the input first instead use dto level trim 
            .NotEmpty()
            .Length(3, 20)
            
            .Matches("^[a-zA-Z ]+$")
            .WithMessage("Name should contain only letters and spaces.")
            // .NotEqual(x => x.Department) // it compare with  same type does not work with (eg. Name = cse,Department = CSE)
            // .Must((dto, name) => name?.ToLower() != dto.Department?.ToLower()
            .Must((dto, name) => !string.Equals(name, dto.Department, StringComparison.OrdinalIgnoreCase))
            .WithMessage("Name and Department cannot be same");

        RuleFor(x => x.Department)
            .NotEmpty()
            .Length(2, 10)
            .Matches("^[A-Z]+$")
            .WithMessage("Department must contain uppercase letters only.");
    }
}