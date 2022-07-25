using Entities.DataTransferObjects.ProductDTO;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.OrderDTO
{
    public class OrderToShowDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }//полное имя покупателя
        public string ContactPhone { get; set; } // контактный телефон покупателя
        public string DeliveryPointAddress { get; set; }//mapping
        public string UserName { get; set; }//mapping
        public virtual ICollection<ProductToShowInOrderDto> Products { get; set; }
        public OrderToShowDto()
        {
            Products = new List<ProductToShowInOrderDto>();
        }
    }
}
