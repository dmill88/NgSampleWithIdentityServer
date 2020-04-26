using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Sample.DomainLogic.Models
{
    public class BlogPostDto: PostDto
    {
        public int BlogId { get; set; }

        public string BlogDisplayName { get; set; }
    }

}
