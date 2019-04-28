namespace Lykke.Service.EasyBuy.Domain.Entities.Orders
{
    /// <summary>
    /// Specifies a transfer type. 
    /// </summary>
    public enum TransferType
    {
        /// <summary>
        /// Unspecified type.
        /// </summary>
        None,

        /// <summary>
        /// Indicates transfer to reserve client funds.
        /// </summary>
        Reserve,

        /// <summary>
        /// Indicates transfer to settle client funds.
        /// </summary>
        Settlement
    }
}
