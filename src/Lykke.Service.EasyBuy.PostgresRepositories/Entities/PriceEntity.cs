using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lykke.Service.EasyBuy.PostgresRepositories.Entities
{
    [Table("prices")]
    public class PriceEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("asset_pair")]
        public string AssetPair { get; set; }

        [Column("value")]
        public decimal Value { get; set; }

        [Column("base_volume")]
        public decimal BaseVolume { get; set; }

        [Column("quote_volume")]
        public decimal QuoteVolume { get; set; }

        [Column("valid_from")]
        public DateTime ValidFrom { get; set; }

        [Column("valid_to")]
        public DateTime ValidTo { get; set; }

        [Column("exchange")]
        public string Exchange { get; set; }

        [Column("original_value")]
        public decimal OriginalValue { get; set; }

        [Column("markup")]
        public decimal Markup { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
