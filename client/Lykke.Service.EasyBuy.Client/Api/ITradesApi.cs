using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models;

namespace Lykke.Service.EasyBuy.Client.Api
{
    /// <summary>
    /// Provides methods to work with executed trades.
    /// </summary>
    [PublicAPI]
    public interface ITradesApi
    {
        /// <summary>
        /// Used to fetch trades.
        /// </summary>
        /// <param name="fromDate">Date From.</param>
        /// <param name="toDate">Date To.</param>
        /// <returns>A list of trades.</returns>
        Task<IReadOnlyList<TradeModel>> GetTradesAsync(DateTime fromDate, DateTime toDate);
    }
}
