using FluentValidation;
using SchoolAPI.Models;

namespace SchoolAPI.Validators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Age).InclusiveBetween(18, 30).WithMessage("Age must be between 18 and 30.");
            
        }
    }
}
