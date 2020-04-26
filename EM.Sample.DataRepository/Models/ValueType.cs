using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class ValueType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FormatString { get; set; }
        public bool? IsActive { get; set; }
    }
}