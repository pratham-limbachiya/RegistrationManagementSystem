using FluentValidation;
using Managementsystem.Model;

namespace Managementsystem.Validaton
{
    public class RegistrationValidator : AbstractValidator<Registration>
    {
        public RegistrationValidator()
            {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(10)
                .WithMessage("Name cannot exceed 10 characters.");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters.");

            When(x => x.Id == 0, () =>
            {
                RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage("Password is required.")
                    .MinimumLength(8)
                    .WithMessage("Password must be at least 8 characters.")
                    .Matches("[A-Z]")
                    .WithMessage("Password must contain uppercase letter.")
                    .Matches("[a-z]")
                    .WithMessage("Password must contain lowercase letter.")
                    .Matches("[0-9]")
                    .WithMessage("Password must contain number.")
                    .Matches(@"[\!\@\#\$\%\^\&\*]")
                    .WithMessage("Password must contain special character.");
            });

            RuleFor(x => x.DateOfBirth)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Date of birth cannot be a future date.");

            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("Gender is required.");

            RuleFor(x => x.Hobbies)
                .NotNull()
                 .WithMessage("Please select at least one hobby.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.");

            RuleFor(x => x.StateId)
                .GreaterThan(0)
                .WithMessage("Please select state.");

            RuleFor(x => x.CityId)
                .GreaterThan(0)
                .WithMessage("Please select city.");

            RuleFor(x => x.Pincode)
                .NotEmpty()
                .WithMessage("Pincode is required.")
                .Matches(@"^[0-9]{6}$")
                .WithMessage("Pincode must contain exactly 6 digits.");

            When(x => x.Id == 0, () =>
            {
                RuleFor(x => x.Files)
                    .NotNull()
                    .Must(files => files != null && files.Count > 0)
                    .WithMessage("Please select at least one file.");
            });

            RuleForEach(x => x.Files)
                .Must(file =>
                    file.ContentType == "image/jpeg" ||
                    file.ContentType == "image/png")
                 .WithMessage("Only JPG and PNG files are allowed.");

                //.Must(file => file.Length <= 2 * 1024 * 1024)
                //.WithMessage("Each file size must not exceed 2 MB.");
        }
    }
}
