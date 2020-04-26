using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class BlogStatus
    {
        public BlogStatus()
        {
            Blog = new HashSet<Blog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<Blog> Blog { get; set; }
    }
}