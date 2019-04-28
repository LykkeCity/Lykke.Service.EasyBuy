namespace Lykke.Service.EasyBuy.Domain.Entities.Balances
{
    /// <summary>
    /// Represents a balance of an asset.
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Balance"/> of asset with zero amount.
        /// </summary>
        /// <param name="asset">The asset name.</param>
        public Balance(string asset)
        {
            Asset = asset;
            Amount = decimal.Zero;
            Reserved = decimal.Zero;
        }
        
        /// <summary>
        /// Initializes a new instance of <see cref="Balance"/> of asset with amount of balance.
        /// </summary>
        /// <param name="asset">The asset name.</param>
        /// <param name="amount">The amount of balance.</param>
        /// <param name="reserved">The amount that currently are reserved.</param>
        public Balance(string asset, decimal amount, decimal reserved)
        {
            Asset = asset;
            Amount = amount;
            Reserved = reserved;
        }

        /// <summary>
        /// The asset id.
        /// </summary>
        public string Asset { get; }

        /// <summary>
        /// The amount of balance.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// The amount that currently are reserved.
        /// </summary>
        public decimal Reserved { get; }
    }
}
