namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents current state of the instrument.
    /// </summary>
    public enum InstrumentState
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None,
        
        /// <summary>
        /// Instrument is currently active.
        /// </summary>
        Active,
        
        /// <summary>
        /// Instrument is currently disabled.
        /// </summary>
        Disabled
    }
}
