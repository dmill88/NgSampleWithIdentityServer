using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EM.Data.Helpers
{
    public class PagedDataState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedDataState"/> class.
        /// </summary>
        public PagedDataState()
        {
            this.SortMembers = new List<SortDescriptor>();
            this.Skip = 0;
            this.Take = 10;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedDataState"/> class.
        /// </summary>
        /// <param name="skip">The number of rows to skip.</param>
        /// <param name="take">The number of rows to return.</param>
        public PagedDataState(int skip, int take)
        {
            this.SortMembers = new List<SortDescriptor>();
            this.Skip = skip;
            this.Take = take;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedDataState"/> class.
        /// </summary>
        /// <param name="skip">The number of rows to skip.</param>
        /// <param name="take">The number of rows to return.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sortDirection">The sort direction.</param>
        public PagedDataState(int skip, int take, string memberName, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            this.Skip = skip;
            this.Take = take;
            this.SortMembers = new List<SortDescriptor>();
            this.SortMembers.Add(new SortDescriptor(memberName, sortDirection));
        }

        private int _skip = 0;
        public int Skip
        {
            get
            {
                if (_skip < 0)
                    _skip = 0;
                return _skip;
            }
            set
            {
                if (value < 0)
                    _skip = 0;
                else
                    _skip = value;
            }
        }

        public int Take { get; set; }

        public List<SortDescriptor> SortMembers { get; }
    }

    public class SortDescriptor
    {
        public SortDescriptor()
        {
            SortDirection = ListSortDirection.Ascending;
        }
        public SortDescriptor(string memberName, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            Field = memberName;
            SortDirection = sortDirection;
        }

        /// <summary>
        /// Represents the property to be sorted
        /// </summary>
        public string Field { get; set; }

        public ListSortDirection SortDirection { get; set; }
    }
}
