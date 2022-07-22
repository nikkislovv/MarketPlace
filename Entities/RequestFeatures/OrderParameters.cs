using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class OrderParameters:RequestParameters
    {
        public string SearchByField { get; set; }//поиск по определенному полю
        public string SearchTerm { get; set; }//значение этого поля
    }
}
