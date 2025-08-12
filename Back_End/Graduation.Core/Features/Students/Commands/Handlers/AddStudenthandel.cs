using AutoMapper;
using Graduation.Core.Bases;
using Graduation.Core.Features.Students.Commands.Models;
using Graduation.Infrustructure.UnitOfWorks;
using Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Features.Students.Commands.Handlers
{
    internal class AddStudenthandel : ResponseHandler, IRequestHandler<AddStudentCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddStudenthandel(IMapper mapper , IUnitOfWork unitOfWork)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
           var student = _mapper.Map<Student>(request);
           await _unitOfWork.Repository<Student>().AddAsync(student);
              await _unitOfWork.CompleteAsync();
            return Success("success");

        }
    }
}
