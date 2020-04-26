using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EM.Membership.Isp.Models
{
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        
        public bool RememberLogin { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}
