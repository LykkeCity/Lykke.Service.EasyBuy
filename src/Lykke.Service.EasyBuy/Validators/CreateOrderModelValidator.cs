using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models;
using Lykke.Service.EasyBuy.Client.Models.Orders;

namespace Lykke.Service.EasyBuy.Validators
{
    [UsedImplicitly]
    public class CreateOrderModelValidator : AbstractValidator<CreateOrderModel>
    {
        public CreateOrderModelValidator()
        {
            RuleFor(o => o.ClientId)
                .NotEmpty()
                .WithMessage("Client Id required.");

            RuleFor(o => o.PriceId)
                .NotEmpty()
                .WithMessage("Price Id required.");

            RuleFor(o => o.QuotingVolume)
                .GreaterThan(0m)
                .WithMessage("Positive quoting volume required.");
        }
    }
}
