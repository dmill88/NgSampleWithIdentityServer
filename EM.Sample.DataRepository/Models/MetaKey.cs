using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class MetaKey
    {
        public MetaKey()
        {
            AuthorMetaData = new HashSet<AuthorMetaData>();
        }

        public int Id { get; set; }
        public string DataKey { get; set; }

        public virtual ICollection<AuthorMetaData> AuthorMetaData { get; set; }
    }
}