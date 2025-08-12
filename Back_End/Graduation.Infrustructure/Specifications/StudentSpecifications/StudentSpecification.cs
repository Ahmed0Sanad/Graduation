using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Infrustructure.Specifications.StudentSpecifications
{
    public class StudentSpecification : Specification<Student>
    {
        public StudentSpecification(StudentSpecificationParm parm )
            : base(x => (string.IsNullOrEmpty(parm.Search) || x.Name.ToLower().Contains(parm.Search)))
   
        {
            #region sorting
            if (parm.sort != null)
            {
                if (parm.sort == "Name" && parm.orderBy == Data.Enums.OrderByOrdring.Ascending)
                {
                    OrderBy = x => x.Name;
                }
                else if (parm.sort == "Name" && parm.orderBy == Data.Enums.OrderByOrdring.Descending)
                {
                    OrderByDesc = x => x.Name;
                }
                else if (parm.sort == "Age" && parm.orderBy == Data.Enums.OrderByOrdring.Ascending)
                {
                    OrderBy = x => x.Age;
                }
                else if (parm.sort == "Age" && parm.orderBy == Data.Enums.OrderByOrdring.Descending)
                {
                    OrderByDesc = x => x.Age;
                }

            }
            #endregion



            ApplyPagination(parm.PageSize, parm.index);
        }

    }
}
