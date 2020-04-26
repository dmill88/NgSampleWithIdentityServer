using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class LoginProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}