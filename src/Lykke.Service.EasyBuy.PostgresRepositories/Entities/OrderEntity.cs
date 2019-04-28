using System;
using System.ComponentModel.DataAnnotations.Schema;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;

namespace Lykke.Service.EasyBuy.PostgresRepositories.Entities
{
    [Table("orders")]
    public class OrderEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Column("price_id")]
        public Guid PriceId { get; set; }

        [Column("asset_pair")]
        public string AssetPair { get; set; }

        [Column("base_volume")]
        public decimal BaseVolume { get; set; }

        [Column("quote_volume")]
        public decimal QuoteVolume { get; set; }

        [Column("status")]
        public OrderStatus Status { get; set; }

        [Column("error")]
        public string Error { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
