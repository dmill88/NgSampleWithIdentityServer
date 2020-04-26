using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EM.MembershipDb
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(120)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(120)]
        public string LastName { get; set; }

        public string Fullname 
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
