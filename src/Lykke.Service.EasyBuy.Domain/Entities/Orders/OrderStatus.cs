namespace Lykke.Service.EasyBuy.Domain.Entities.Orders
{
    /// <summary>
    /// Specifies an order status.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Unspecified status.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the order is not processed yet.
        /// </summary>
        New,

        /// <summary>
        /// Indicates that client funds are successfully reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// Indicates that funds are successfully transferred to the client.
        /// </summary>
        Completed,

        /// <summary>
        /// Indicated that order not processed.
        /// </summary>
        Cancelled
    }
}
