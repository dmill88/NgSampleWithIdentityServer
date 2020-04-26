using System;
using System.Collections.Generic;
using System.ComponentModel;
using EM.Data.Helpers;

namespace EM.Sample.DomainLogic.Filters
{
    public class BlogPostsFilter: PagedDataState
    {
        public BlogPostsFilter()
        {
            this.Skip = 0;
            this.Take = 10;
            this.SortMembers.Add(new SortDescriptor("UpdatedAt", ListSortDirection.Ascending));
        }

        public int BlogId { get; set; }

        public string Title { get; set; }
    }
}
