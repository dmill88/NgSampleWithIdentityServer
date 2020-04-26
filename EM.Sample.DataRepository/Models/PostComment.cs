using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class PostComment
    {
        public PostComment()
        {
            InverseParent = new HashSet<PostComment>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public int? ApprovedByAuthorId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Author ApprovedByAuthor { get; set; }
        public virtual Author Author { get; set; }
        public virtual PostComment Parent { get; set; }
        public virtual Post Post { get; set; }
        public virtual ICollection<PostComment> InverseParent { get; set; }
    }
}