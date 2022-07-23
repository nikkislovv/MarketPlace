using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.AccountDTO
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(15, ErrorMessage = "Cant be more than 15")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password name is required")]
        public string Password { get; set; }
    }
}
