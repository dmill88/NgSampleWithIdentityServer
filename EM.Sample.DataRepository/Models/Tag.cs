using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class Tag
    {
        public Tag()
        {
            BlogTag = new HashSet<BlogTag>();
            PostTag = new HashSet<PostTag>();
            TagGroupMember = new HashSet<TagGroupMember>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<BlogTag> BlogTag { get; set; }
        public virtual ICollection<PostTag> PostTag { get; set; }
        public virtual ICollection<TagGroupMember> TagGroupMember { get; set; }
    }
}