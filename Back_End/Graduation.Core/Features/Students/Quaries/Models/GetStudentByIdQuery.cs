using Graduation.Core.Bases;
using Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Features.Students.Quaries.Models
{
    public class GetStudentByIdQuery : IRequest<Response<Student>>
    {
        public int Id { get; set; }
    }
}
