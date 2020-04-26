using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class Category
    {
        public Category()
        {
            PostCategory = new HashSet<PostCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<PostCategory> PostCategory { get; set; }
    }
}