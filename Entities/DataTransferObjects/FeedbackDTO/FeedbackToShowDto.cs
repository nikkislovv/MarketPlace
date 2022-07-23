using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.FeedbackDTO
{
    public class FeedbackToShowDto
    {
        public Guid Id { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public string ProductName { get; set; }//mapping
        public string UserName { get; set; }//mapping
    }
}
