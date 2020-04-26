using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class PostCategory
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int CategoryId { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Category Category { get; set; }
        public virtual Post Post { get; set; }
    }
}