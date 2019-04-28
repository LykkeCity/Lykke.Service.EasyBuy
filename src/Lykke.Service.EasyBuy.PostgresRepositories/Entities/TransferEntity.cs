using System;
using System.ComponentModel.DataAnnotations.Schema;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;

namespace Lykke.Service.EasyBuy.PostgresRepositories.Entities
{
    [Table("transfers")]
    public class TransferEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("order_id")]
        public Guid OrderId { get; set; }

        [Column("type")]
        public TransferType Type { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
