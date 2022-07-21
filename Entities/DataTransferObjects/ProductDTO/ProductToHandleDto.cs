using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.ProductDTO
{
    public class ProductToHandleDto
    {
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is a required field.")]
        [Range(1, double.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Quantity is a required field.")]
        [Range(1, double.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Description is a required field.")]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required(ErrorMessage = "WarehouseId is a required field.")]
        public Guid WarehouseId { get; set; }
    }
}
