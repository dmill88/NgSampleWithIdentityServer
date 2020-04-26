using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EM.Sample.DomainLogic.Models
{
    public class AuthorDto
    {
        [Key]       
        public int Id { get; set; }

        [MaxLength(450)]
        public string UserId { get; set; }

        [MaxLength(150)]
        public string FirstName { get; set; }

        [MaxLength(150)]
        public string LastName { get; set; }

        [MaxLength(150)]
        public string Alias { get; set; }

        public string Bio { get; set; }

        public bool Active { get; set; } = true;

        public string DisplayName { 
            get 
            {
                string displayName = Alias;
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = $"{FirstName} {LastName}";
                }
                return displayName;
            }
        }
    }
}
