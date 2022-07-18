using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }//полное имя покупателя
        public string ContactPhone { get; set; } // контактный телефон покупателя
        public Guid DeliveryPointId { get; set; } // точка выдачи товара(выбирается покупателем)
        public virtual DeliveryPoint DeliveryPoint { get; set; }
        public string UserId { get; set; }//аккаунт с которого осуществляется покупка
        public virtual User User { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public Order()
        {
            Products = new List<Product>();
        }
    }
}
