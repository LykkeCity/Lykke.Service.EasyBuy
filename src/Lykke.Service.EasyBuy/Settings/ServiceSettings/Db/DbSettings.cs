using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings.Db
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DbSettings
    {
        public string DataConnectionString { get; set; }

        [AzureTableCheck]
        public string LogsConnectionString { get; set; }
    }
}
