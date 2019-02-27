using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models;

namespace Lykke.Service.EasyBuy.Validators
{
    [UsedImplicitly]
    public class CalculatePriceModelValidator : AbstractValidator<CalculatePriceModel>
    {
        public CalculatePriceModelValidator()
        {
            RuleFor(o => o.AssetPair)
                .NotEmpty()
                .WithMessage("Asset pair required.");
            
            RuleFor(o => o.WalletId)
                .NotEmpty()
                .WithMessage("Exchange required.");
            
            RuleFor(o => o.QuotingVolume)
                .GreaterThan(0)
                .WithMessage("Non-zero quoting volume required.");
            
            RuleFor(o => o.Type)
                .Must(o => o != OrderType.None)
                .WithMessage("Order type required.");
        }
    }
}
