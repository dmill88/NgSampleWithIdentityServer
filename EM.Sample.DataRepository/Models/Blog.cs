using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class Blog
    {
        public Blog()
        {
            BlogPost = new HashSet<BlogPost>();
            BlogTag = new HashSet<BlogTag>();
        }

        public int Id { get; set; }
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public int PrimaryAuthorId { get; set; }
        public int BlogStatusId { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }

        public virtual BlogStatus BlogStatus { get; set; }
        public virtual Author PrimaryAuthor { get; set; }
        public virtual ICollection<BlogPost> BlogPost { get; set; }
        public virtual ICollection<BlogTag> BlogTag { get; set; }
    }
}