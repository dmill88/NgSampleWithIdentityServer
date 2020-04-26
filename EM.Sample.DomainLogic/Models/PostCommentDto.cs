using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Sample.DomainLogic.Models
{
    public class PostCommentDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public string AuthorAlias { get; set; }
        public int? ApprovedByAuthorId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
