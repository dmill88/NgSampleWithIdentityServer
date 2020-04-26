using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class TagGroupMember
    {
        public int Id { get; set; }
        public int TagGroupId { get; set; }
        public int TagId { get; set; }
        public int TagOrder { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual TagGroup TagGroup { get; set; }
    }
}