using System;
using System.Collections;

namespace EM.Data.Helpers
{
    public class PagedDataResult
    {
        public PagedDataResult()
        {
        }
        public PagedDataResult(IEnumerable data, int total)
        {
            Data = data;
            Total = total;
        }

        /// <summary>
        /// The data collection.
        /// </summary>
        public IEnumerable Data { get; set; }


        /// <summary>
        /// The total number of data entries.
        /// </summary>
        public int Total { get; set; }

    }
}
