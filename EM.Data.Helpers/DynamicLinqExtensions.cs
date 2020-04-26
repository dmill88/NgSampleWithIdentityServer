using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace EM.Data.Helpers
{
    public static class DynamicLinqExtensions
    {
        public static IQueryable<T> ApplySortToEntities<T>(this IQueryable<T> entities, List<SortDescriptor> sort)
        {
            List<string> sortDescriptionFields = new List<string>();
            foreach (SortDescriptor sd in sort)
            {
                if (sd.SortDirection == System.ComponentModel.ListSortDirection.Ascending)
                {
                    sortDescriptionFields.Add(sd.Field);
                }
                else
                {
                    sortDescriptionFields.Add($"{sd.Field} descending");
                }
            }

            if (sortDescriptionFields.Count > 0)
            {
                string sortExpression = string.Join(",", sortDescriptionFields);
                entities = entities.OrderBy(sortExpression);
            }

            return entities;
        }

    }
}
