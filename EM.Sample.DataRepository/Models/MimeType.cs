using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class MimeType
    {
        public MimeType()
        {
            Image = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string MediaType { get; set; }
        public string Suffix { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<Image> Image { get; set; }
    }
}