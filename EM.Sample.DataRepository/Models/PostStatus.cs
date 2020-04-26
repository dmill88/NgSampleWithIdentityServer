using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class PostStatus
    {
        public PostStatus()
        {
            Post = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}