using System;
using System.Collections.Generic;
using System.ComponentModel;
using EM.Data.Helpers;


namespace EM.Sample.DomainLogic.Filters
{
    public class AuthorFilter: PagedDataState
    {
        public AuthorFilter()
        {
            this.SortMembers.Add(new SortDescriptor("LastName", ListSortDirection.Ascending));
            this.SortMembers.Add(new SortDescriptor("FirstName", ListSortDirection.Ascending));
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Alias { get; set; }

    }
}
