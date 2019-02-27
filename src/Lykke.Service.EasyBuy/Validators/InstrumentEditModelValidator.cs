using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models;

namespace Lykke.Service.EasyBuy.Validators
{
    [UsedImplicitly]
    public class InstrumentEditModelValidator : AbstractValidator<InstrumentEditModel>
    {
        public InstrumentEditModelValidator()
        {
            RuleFor(o => o.AssetPair)
                .NotEmpty()
                .WithMessage("Asset pair required.");
            
            RuleFor(o => o.Exchange)
                .NotEmpty()
                .WithMessage("Exchange required.");
            
            RuleFor(o => o.State)
                .Must(o => o != InstrumentState.None)
                .WithMessage("Instrument state required.");
        }
    }
}
