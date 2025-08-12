using Graduation.Core.Features.Students.Quaries.Results;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void GetStudentListMapping() {
            CreateMap<Student, GetStudentListResponse>().ForMember(des => des.stuName, o => o.MapFrom(o => "student: " + o.Name)).ReverseMap();
        }
    }
}
