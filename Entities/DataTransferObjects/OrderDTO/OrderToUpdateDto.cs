using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.OrderDTO
{
    public class OrderToUpdateDto
    {
        [Required(ErrorMessage = "FullName is a required field.")]
        [MaxLength(50)]
        public string FullName { get; set; }//полное имя покупателя
        [Required(ErrorMessage = "ContactPhone is a required field.")]
        public string ContactPhone { get; set; } // контактный телефон покупателя
        [Required(ErrorMessage = "DeliveryPointId is a required field.")]
        public Guid DeliveryPointId { get; set; } // точка выдачи товара(выбирается покупателем)
    }
}
