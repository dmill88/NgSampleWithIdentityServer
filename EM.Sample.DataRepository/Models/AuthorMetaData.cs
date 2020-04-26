using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class AuthorMetaData
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int MetaKeyId { get; set; }
        public string MetaValue { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Author Author { get; set; }
        public virtual MetaKey MetaKey { get; set; }
    }
}