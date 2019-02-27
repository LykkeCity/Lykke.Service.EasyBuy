using FluentValidation;
using Lykke.Service.EasyBuy.Client.Models;

namespace Lykke.Service.EasyBuy.Validators
{
    public class CreateOrderModelValidator : AbstractValidator<CreateOrderModel>
    {
        public CreateOrderModelValidator()
        {
            RuleFor(o => o.WalletId)
                .NotEmpty()
                .WithMessage("Wallet Id required.");
            
            RuleFor(o => o.PriceId)
                .NotEmpty()
                .WithMessage("Price Id required.");
            
            RuleFor(o => o.QuotingVolume)
                .GreaterThan(0)
                .WithMessage("Positive quoting volume required.");
        }
    }
}
