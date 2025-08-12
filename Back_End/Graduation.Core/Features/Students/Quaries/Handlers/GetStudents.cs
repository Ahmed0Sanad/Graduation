using AutoMapper;
using Graduation.Core.Bases;
using Graduation.Core.Features.Students.Quaries.Models;
using Graduation.Core.Features.Students.Quaries.Results;
using Graduation.Core.Resources;
using Graduation.Core.Warppars;
using Graduation.Infrustructure.Abstract;
using Graduation.Infrustructure.Specifications.StudentSpecifications;
using Graduation.Infrustructure.UnitOfWorks;
using Graduation.Service.Abstract;
using Data.Entities;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Features.Students.Quaries.Handlers
{
    public class GetStudents : ResponseHandler,
        IRequestHandler<GetStudentList, Response<PaginatedResult<GetStudentListResponse>>>,
        IRequestHandler<GetStudentByIdQuery,Response<Student>>
    {
        private readonly IStudentService _studentservice;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public GetStudents(IStudentService studentservice , IMapper mapper , IUnitOfWork unitOfWork , IStringLocalizer<SharedResources> stringLocalizer)
        {
            _studentservice = studentservice;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this._stringLocalizer = stringLocalizer;
        }
        public async Task<Response<PaginatedResult<GetStudentListResponse>>> Handle(GetStudentList request, CancellationToken cancellationToken)
        {
            var spec = new StudentSpecification(request.parm);
            var students =  await unitOfWork.Repository<Student>().GetAllSpecAsync(spec);
            var studentsToReturn = mapper.Map<IEnumerable<GetStudentListResponse>>(students);

            var count = await unitOfWork.Repository<Student>().CountSpecAsync(spec.Criteria);

            var pagedResult = new PaginatedResult<GetStudentListResponse>(data: studentsToReturn.ToList()
                ,
                page: request.parm.index,
                pageSize: request.parm.PageSize,
                count: count);

            return Success(pagedResult);
        }

        public async Task<Response<Student>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.Repository<Student>().GetByIdAsync(request.Id);
            if (result != null) {
                return Success(result);
            }
            return NotFound<Student>(_stringLocalizer[SharedResourcesKeys.NotFound]);
        }
    }
    
    
}
