using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.FeedbackDTO
{
    public class FeedbackToCreateDto
    {
        public double Rating { get; set; }
        public string Comment { get; set; }
    }
}
