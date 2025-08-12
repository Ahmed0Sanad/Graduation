using AutoMapper;
using Graduation.Core.Features.Students.Quaries.Models;
using Graduation.Core.Features.Students.Quaries.Results;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Mapping.Students
{
    public partial class StudentProfile : Profile
    {
        public StudentProfile()
        {
            GetStudentListMapping();
            AddStudentCommandMapping();
        }
    }
}
