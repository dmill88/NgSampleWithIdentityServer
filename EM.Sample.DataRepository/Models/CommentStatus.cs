using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class CommentStatus
    {
        public CommentStatus()
        {
            Post = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}