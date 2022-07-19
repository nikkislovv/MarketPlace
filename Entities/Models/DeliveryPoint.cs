using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class DeliveryPoint//пункт доставки товаров
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
