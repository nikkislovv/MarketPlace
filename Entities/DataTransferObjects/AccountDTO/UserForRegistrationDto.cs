using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.AccountDTO
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [MaxLength(20, ErrorMessage = "Cant be more than 20")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
