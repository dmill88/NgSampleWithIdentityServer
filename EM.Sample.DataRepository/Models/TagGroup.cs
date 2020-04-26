using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class TagGroup
    {
        public TagGroup()
        {
            TagGroupMember = new HashSet<TagGroupMember>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<TagGroupMember> TagGroupMember { get; set; }
    }
}