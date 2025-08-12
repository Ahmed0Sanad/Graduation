using Data.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Infrustructure.Specifications
{
    public class Specification<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
        public bool IsPagination { get; set; }

        public Specification()
        {

        }
        public Specification(Expression<Func<T, bool>> cretira)
        {
            Criteria = cretira;

        }
        protected void ApplyPagination(int pageSize, int index)
        {
           

            IsPagination = true;
            this.skip = (index - 1) * pageSize;
            this.take = pageSize;
        }
    }

}
