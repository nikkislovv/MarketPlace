﻿using Entities.DataTransferObjects.FeedbackDTO;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.ProductDTO
{
    public class ProductToShowDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }//серфис сделать для сверки
        public string Description { get; set; }
        public string WarehouseAddress { get; set; }//mapping
        public string SellerName { get; set; }//mapping
        public virtual ICollection<FeedbackToShowDto> Feedbacks { get; set; }//mapping
        public ProductToShowDto()
        {
            Feedbacks = new List<FeedbackToShowDto>();
        }
    }

}
