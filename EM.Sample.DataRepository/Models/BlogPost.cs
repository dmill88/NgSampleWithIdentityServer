using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class BlogPost
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Post Post { get; set; }
    }
}