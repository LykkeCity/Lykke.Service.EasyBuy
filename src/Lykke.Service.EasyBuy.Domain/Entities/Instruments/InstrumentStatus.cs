namespace Lykke.Service.EasyBuy.Domain.Entities.Instruments
{
    /// <summary>
    /// Specifies an instrument status. 
    /// </summary>
    public enum InstrumentStatus
    {
        /// <summary>
        /// Unspecified status.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the instrument is active and price could be provided.
        /// </summary>
        Active,

        /// <summary>
        /// Indicates that the instrument is disable, the price provisioning is stopped.
        /// </summary>
        Disabled
    }
}
