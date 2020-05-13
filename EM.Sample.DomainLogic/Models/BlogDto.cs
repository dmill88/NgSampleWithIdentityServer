using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EM.Sample.DomainLogic.Enums;

namespace EM.Sample.DomainLogic.Models
{
    public class BlogDto
    {
        public BlogDto()
        {
            GUID = Guid.NewGuid();
            BlogStatusId = (int)BlogStatuses.Draft;
        }

        [Key]
        public int Id { get; set; }

        public Guid GUID { get; set; }
        
        [MaxLength(350), Display(Name = "Name"), Required]
        public string Name { get; set; }
        
        public int PrimaryAuthorId { get; set; }

        public AuthorDto PrimaryAuthor { get; set; }

        public int BlogStatusId { get; set; }

        public BlogStatusDto BlogStatus { get; set; }

        [MaxLength(350), Display(Name = "Display Name"), Required]
        public string DisplayName { get; set; }

        [Display(Name = "Display Order"), Range(0, 10000)]
        public int DisplayOrder { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}
