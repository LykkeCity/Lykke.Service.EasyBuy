using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models.Settings
{
    /// <summary>
    /// Represents details about EasyBuy wallet.
    /// </summary>
    [PublicAPI]
    public class AccountSettingsModel
    {
        /// <summary>
        /// Wallet Id which EasyBuy wallet uses.
        /// </summary>
        public string WalletId { set; get; }
    }
}
