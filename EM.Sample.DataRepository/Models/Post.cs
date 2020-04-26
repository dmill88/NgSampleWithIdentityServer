using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class Post
    {
        public Post()
        {
            BlogPost = new HashSet<BlogPost>();
            PostAuthor = new HashSet<PostAuthor>();
            PostCategory = new HashSet<PostCategory>();
            PostComment = new HashSet<PostComment>();
            PostTag = new HashSet<PostTag>();
        }

        public int Id { get; set; }
        public Guid GUID { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string Excerpt { get; set; }
        public int PostStatusId { get; set; }
        public int CommentStatusId { get; set; }
        public int CommentCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual CommentStatus CommentStatus { get; set; }
        public virtual PostStatus PostStatus { get; set; }
        public virtual ICollection<BlogPost> BlogPost { get; set; }
        public virtual ICollection<PostAuthor> PostAuthor { get; set; }
        public virtual ICollection<PostCategory> PostCategory { get; set; }
        public virtual ICollection<PostComment> PostComment { get; set; }
        public virtual ICollection<PostTag> PostTag { get; set; }
    }
}