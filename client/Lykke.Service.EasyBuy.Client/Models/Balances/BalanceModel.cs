using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models.Balances
{
    /// <summary>
    /// Represents balance item.
    /// </summary>
    [PublicAPI]
    public class BalanceModel
    {
        /// <summary>
        /// The asset id.
        /// </summary>
        public string Asset { get; set; }

        /// <summary>
        /// The amount of balance.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The amount that currently are reserved.
        /// </summary>
        public decimal Reserved { get; set; }
    }
}
