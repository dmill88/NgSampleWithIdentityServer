using EM.Sample.DomainLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EM.Sample.DomainLogic.Models
{
    public class PostDto
    {
        public PostDto()
        {
            GUID = Guid.NewGuid();
            UpdatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public int PrimaryAuthorId { get; set; }

        public Guid GUID { get; set; }

        [MaxLength(450)]
        public string Title { get; set; }

        [Display(Name="Post")]
        public string PostContent { get; set; }

        [MaxLength(1000)]
        public string Excerpt { get; set; }

        public int PostStatusId { get; set; } = (int)PostStatuses.Draft;

        public int CommentStatusId { get; set; } = (int)Enums.CommentStatuses.MemberOnly;

        public int CommentCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<string> Tags { get; set; } = new List<string>();

    }
}
