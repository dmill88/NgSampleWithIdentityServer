using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class Author
    {
        public Author()
        {
            AuthorMetaData = new HashSet<AuthorMetaData>();
            Blog = new HashSet<Blog>();
            Image = new HashSet<Image>();
            PostAuthor = new HashSet<PostAuthor>();
            PostCommentApprovedByAuthor = new HashSet<PostComment>();
            PostCommentAuthor = new HashSet<PostComment>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Alias { get; set; }
        public string Bio { get; set; }
        public int? PortraitImgId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool? Active { get; set; }

        public virtual Image PortraitImg { get; set; }
        public virtual ICollection<AuthorMetaData> AuthorMetaData { get; set; }
        public virtual ICollection<Blog> Blog { get; set; }
        public virtual ICollection<Image> Image { get; set; }
        public virtual ICollection<PostAuthor> PostAuthor { get; set; }
        public virtual ICollection<PostComment> PostCommentApprovedByAuthor { get; set; }
        public virtual ICollection<PostComment> PostCommentAuthor { get; set; }
    }
}