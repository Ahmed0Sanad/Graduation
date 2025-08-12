using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Infrustructure.Specifications.StudentSpecifications
{
    public class StudentSpecificationParm
    {
        private int pagesize = 1;
        public string? sort { get; set; }

        public OrderByOrdring? orderBy { get; set; } = OrderByOrdring.Ascending;
        public int PageSize
        {
            get { return pagesize; }
            set
            {
                pagesize = value > 10 ? 10 : value;
            }
        }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        public int index { get; set; } = 1;
    }
}
