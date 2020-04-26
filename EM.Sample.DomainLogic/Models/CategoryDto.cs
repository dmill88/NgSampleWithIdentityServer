using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EM.Sample.DomainLogic.Models
{
    public class CategoryDto
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(500), Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        public bool Active { get; set; } = true;
    }
}
