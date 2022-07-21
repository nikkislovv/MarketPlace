using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class ProductParameters : RequestParameters
    {
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; } = int.MaxValue;
        public bool ValidPriceRange
        {
            get
            {
                return MaxPrice > MinPrice;
            }
        }
        public string SearchByField { get; set; }//поиск по определенному полю
        public string SearchTerm { get; set; }//значение этого поля

    }
}
