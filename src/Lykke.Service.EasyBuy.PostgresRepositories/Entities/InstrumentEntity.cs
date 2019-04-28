using System;
using System.ComponentModel.DataAnnotations.Schema;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;

namespace Lykke.Service.EasyBuy.PostgresRepositories.Entities
{
    [Table("instruments")]
    public class InstrumentEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("asset_pair")]
        public string AssetPair { get; set; }

        [Column("exchange")]
        public string Exchange { get; set; }

        [Column("lifetime")]
        public TimeSpan Lifetime { get; set; }

        [Column("overlap_time")]
        public TimeSpan OverlapTime { get; set; }

        [Column("markup")]
        public decimal Markup { get; set; }

        [Column("min_quote_volume")]
        public decimal MinQuoteVolume { get; set; }

        [Column("max_quote_volume")]
        public decimal MaxQuoteVolume { get; set; }

        [Column("status")]
        public InstrumentStatus Status { get; set; }
    }
}
