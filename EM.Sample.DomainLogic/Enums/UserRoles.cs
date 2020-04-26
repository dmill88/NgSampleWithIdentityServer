using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EM.Sample.DomainLogic.Enums
{
    public enum UserRoles
    {
        [Display(Name = "Administrator")]
        Admin,

        [Display(Name = "Blog Editor")]
        BlogEditor
    }

}
