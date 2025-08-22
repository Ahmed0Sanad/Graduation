using AutoMapper;
using Data.Entities;
using Graduation.Core.Features.Students.Commands.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Mapping.Students
{
    public partial class StudentProfile : Profile
    {
        public void AddStudentCommandMapping()
        {
            CreateMap<AddStudentCommand, Student>();
             
        }
    }
}
