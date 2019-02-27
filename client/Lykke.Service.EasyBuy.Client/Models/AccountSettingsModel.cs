using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents details about EasyBuy wallet.
    /// </summary>
    [PublicAPI]
    public class AccountSettingsModel
    {
        /// <summary>
        /// Client Id which EasyBuy wallet uses.
        /// </summary>
        public string ClientId { set; get; }
    }
}
