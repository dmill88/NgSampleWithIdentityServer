using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class PostTag
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int TagId { get; set; }
        public int TagOrder { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }
    }
}