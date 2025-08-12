using Data.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Graduation.Infrustructure.Specifications
{
    public static class SpecificaionQueryBuilder
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(this IQueryable<TEntity> inputQuery, Specification<TEntity> specification) where TEntity : BaseModel
        {

            var quary = inputQuery;
            //where
            if (specification.Criteria != null)
            {
                quary = quary.Where(specification.Criteria);

            }

            //orderby
            if (specification.OrderBy != null)
            {
                quary = quary.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDesc != null)
            {
                quary = quary.OrderByDescending(specification.OrderByDesc);
            }

            //skip-take
            if (specification.IsPagination)
            {
                quary = quary.Skip(specification.skip).Take(specification.take);
            }

            quary = specification.Includes.Aggregate(quary, (q, s) => q.Include(s));
            return quary;
        }
    }
}
