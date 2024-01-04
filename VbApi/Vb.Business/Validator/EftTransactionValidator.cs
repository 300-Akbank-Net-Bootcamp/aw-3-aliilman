using FluentValidation;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Validator;

public class CreateEftTransactionValidator : AbstractValidator<EftTransactionRequest>
{
    public CreateEftTransactionValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithName("Account identity number");
        RuleFor(x => x.ReferenceNumber).NotEmpty().MaximumLength(10).WithMessage("ReferenceNumber Cannot exceed 10 characters");
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Amount must be greater than or equal to 0.");
        RuleFor(x => x.SenderAccount).NotEmpty();
        RuleFor(x => x.SenderIban).NotEmpty().MinimumLength(28).MaximumLength(28);
        RuleFor(x => x.SenderName).NotEmpty().MaximumLength(15);
    }
}