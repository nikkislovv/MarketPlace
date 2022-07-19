using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Feedback//отзывы покупателей
    {
        public Guid Id { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string UserId { get; set; }//аккаунт с которого писался отзыв
        public virtual User User { get; set; }
    }
}
