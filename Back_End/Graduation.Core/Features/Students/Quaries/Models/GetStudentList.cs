using Graduation.Core.Bases;
using Graduation.Core.Features.Students.Quaries.Results;
using Graduation.Core.Warppars;
using Graduation.Infrustructure.Specifications.StudentSpecifications;
using Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Features.Students.Quaries.Models
{
    public class GetStudentList:IRequest<Response<PaginatedResult<GetStudentListResponse>>>
    {
        public StudentSpecificationParm  parm { get; set; }
    }
}
