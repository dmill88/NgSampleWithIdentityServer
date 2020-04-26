using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class BlogCategory
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}