using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Musaca.Models
{
    public class Order
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Products = new HashSet<Product>();
        }

        [Key]
        public string Id { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Active;

        public DateTime IssuedOn { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        [Required]
        public string CashierId { get; set; }

        public User Cashier { get; set; }
    }
}