using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class BlogTag
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int TagId { get; set; }
        public int TagOrder { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Tag Tag { get; set; }
    }
}