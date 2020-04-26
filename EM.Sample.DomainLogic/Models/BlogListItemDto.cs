using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EM.Sample.DomainLogic.Models
{
    public class BlogListItemDto: ListItemBase
    {
        public Guid GUID { get; set; }

        [Display(Name="Display Name")]
        public string DisplayName { get; set; }
    }
}
