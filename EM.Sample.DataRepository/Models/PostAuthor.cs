using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class PostAuthor
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public bool IsPrimary { get; set; }
        public int ListOrder { get; set; }

        public virtual Author Author { get; set; }
        public virtual Post Post { get; set; }
    }
}