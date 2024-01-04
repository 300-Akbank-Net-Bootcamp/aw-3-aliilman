using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validator;

public class CreateAccountValidator : AbstractValidator<AccountRequest>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.IBAN).NotEmpty().MaximumLength(26).MinimumLength(26);
        RuleFor(x => x.Balance).NotEmpty();
        RuleFor(x => x.CurrencyType).NotEmpty().MaximumLength(3).MinimumLength(3).WithName("Currency Type");
        RuleFor(x => x.Name).NotEmpty().WithName("Account Name");
        RuleFor(x => x.CustomerId).NotEmpty().WithName("Account Owner Customer ID");
    }
}