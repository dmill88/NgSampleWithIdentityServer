using System;
using System.Collections.Generic;

namespace EM.Sample.DataRepository.Models
{
    public partial class Image
    {
        public Image()
        {
            Author = new HashSet<Author>();
        }

        public int Id { get; set; }
        public int UploaderAuthorId { get; set; }
        public string OriginalImageFilename { get; set; }
        public string BlobUri { get; set; }
        public string BlobImageThumbnailUri { get; set; }
        public string BlobFullImageUri { get; set; }
        public int MimeTypeId { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual MimeType MimeType { get; set; }
        public virtual Author UploaderAuthor { get; set; }
        public virtual ICollection<Author> Author { get; set; }
    }
}