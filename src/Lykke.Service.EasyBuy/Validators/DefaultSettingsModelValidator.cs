using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models;

namespace Lykke.Service.EasyBuy.Validators
{
    [UsedImplicitly]
    public class DefaultSettingsModelValidator : AbstractValidator<DefaultSettingsModel>
    {
        public DefaultSettingsModelValidator()
        {
            RuleFor(o => o.Markup)
                .ExclusiveBetween(0, 1)
                .WithMessage("Markup should be between 0 and 1.");
            
            RuleFor(o => o.OverlapTime)
                .Must(o => o.HasValue)
                .WithMessage("Overlap Time should be present.");
            
            RuleFor(o => o.PriceLifetime)
                .Must(o => o.HasValue)
                .WithMessage("Price lifetime should be present.");
            
            RuleFor(o => o.RecalculationInterval)
                .Must((m, o) => o.HasValue && o.Value < m.PriceLifetime)
                .WithMessage("Recalculation interval should be less than price lifetime.");
            
            RuleFor(o => o.OrdersProcessorPeriod)
                .Must(o => o.HasValue)
                .WithMessage("Timer period should be present.");
        }
    }
}
