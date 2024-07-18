using FluentValidation;
using SchoolAPI.Models;

namespace SchoolAPI.Validators
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            
        }
    }
}
