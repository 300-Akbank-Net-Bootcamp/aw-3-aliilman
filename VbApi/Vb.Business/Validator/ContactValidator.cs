using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validator;

public class CreateContactValidator : AbstractValidator<ContactRequest>
{
    public CreateContactValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithName("Customer identity number");
        RuleFor(x => x.ContactType).NotEmpty().MaximumLength(10).WithMessage("ContactType Cannot exceed 10 characters");
        RuleFor(x => x.Information).NotEmpty().MaximumLength(20).WithMessage("Information Cannot exceed 20 characters");
        RuleFor(x => x.IsDefault).NotEmpty();

    }
}