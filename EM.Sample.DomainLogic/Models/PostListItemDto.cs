using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EM.Sample.DomainLogic.Models
{
    public class PostListItemDto
    {
        public int Id { get; set; }

        [MaxLength(450)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Excerpt { get; set; }

        public int PostStatusId { get; set; }

        public int CommentStatusId { get; set; }

        public int CommentCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
